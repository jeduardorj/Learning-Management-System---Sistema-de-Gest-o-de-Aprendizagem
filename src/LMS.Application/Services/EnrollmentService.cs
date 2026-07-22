using AutoMapper;
using LMS.Application.DTOs.Enrollments;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;

namespace LMS.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<EnrollmentResponseDto> EnrollAsync(Guid courseId)
    {
        var userId = _currentUser.UserId;

        var course = await _unitOfWork.Courses.GetByIdAsync(courseId)
            ?? throw new KeyNotFoundException($"Curso com Id '{courseId}' não encontrado.");

        if (!course.IsActive)
            throw new InvalidOperationException("Não é possível se matricular em um curso inativo.");

        var alreadyEnrolled = await _unitOfWork.Enrollments.ExistsAsync(userId, courseId);
        if (alreadyEnrolled)
            throw new InvalidOperationException("Você já está matriculado neste curso.");

        var enrollment = new Enrollment(userId, courseId);

        await _unitOfWork.Enrollments.AddAsync(enrollment);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<EnrollmentResponseDto>(enrollment);
    }

    public async Task<IEnumerable<EnrollmentWithCourseDto>> GetMyEnrollmentsAsync()
    {
        var userId = _currentUser.UserId;
        var enrollments = await _unitOfWork.Enrollments.GetByUserIdWithCourseAsync(userId);
        return _mapper.Map<IEnumerable<EnrollmentWithCourseDto>>(enrollments);
    }

    public async Task<IEnumerable<EnrollmentResponseDto>> GetByCourseIdAsync(Guid courseId)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId)
            ?? throw new KeyNotFoundException($"Curso com Id '{courseId}' não encontrado.");

        var enrollments = await _unitOfWork.Enrollments.GetByCourseIdAsync(courseId);
        return _mapper.Map<IEnumerable<EnrollmentResponseDto>>(enrollments);
    }

    public async Task UnenrollAsync(Guid courseId)
    {
        var userId = _currentUser.UserId;

        var enrollment = await _unitOfWork.Enrollments.GetByUserAndCourseAsync(userId, courseId)
            ?? throw new KeyNotFoundException("Matrícula não encontrada.");

        _unitOfWork.Enrollments.Delete(enrollment);
        await _unitOfWork.CommitAsync();
    }
}
