using AutoMapper;
using LMS.Application.DTOs.Progress;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;

namespace LMS.Application.Services;

public class ProgressService : IProgressService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public ProgressService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ProgressResponseDto> MarkLessonAsCompletedAsync(Guid courseId, Guid lessonId)
    {
        var userId = _currentUser.UserId;

        var enrollment = await _unitOfWork.Enrollments.GetByUserAndCourseAsync(userId, courseId)
            ?? throw new InvalidOperationException("Você não está matriculado neste curso.");

        var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId)
            ?? throw new KeyNotFoundException($"Aula com Id '{lessonId}' não encontrada.");

        var module = await _unitOfWork.Modules.GetByIdAsync(lesson.ModuleId)
            ?? throw new KeyNotFoundException("Módulo não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("A aula não pertence a este curso.");

        var existing = await _unitOfWork.Progresses
            .GetByEnrollmentAndLessonAsync(enrollment.Id, lessonId);

        if (existing is not null)
        {
            if (!existing.Completed)
            {
                existing.Complete();
                _unitOfWork.Progresses.Update(existing);
                await _unitOfWork.CommitAsync();
                await TryIssueCertificateAsync(userId, courseId, enrollment.Id);
            }

            var existingResult = await _unitOfWork.Progresses
                .GetByEnrollmentAndLessonAsync(enrollment.Id, lessonId);
            return _mapper.Map<ProgressResponseDto>(existingResult!);
        }

        var progress = new Progress(enrollment.Id, lessonId);
        progress.Complete();

        await _unitOfWork.Progresses.AddAsync(progress);
        await _unitOfWork.CommitAsync();

        await TryIssueCertificateAsync(userId, courseId, enrollment.Id);

        var result = await _unitOfWork.Progresses
            .GetByEnrollmentAndLessonAsync(enrollment.Id, lessonId);

        return _mapper.Map<ProgressResponseDto>(result!);
    }

    public async Task<CourseProgressDto> GetCourseProgressAsync(Guid courseId)
    {
        var userId = _currentUser.UserId;

        var course = await _unitOfWork.Courses.GetByIdAsync(courseId)
            ?? throw new KeyNotFoundException($"Curso com Id '{courseId}' não encontrado.");

        var enrollment = await _unitOfWork.Enrollments.GetByUserAndCourseAsync(userId, courseId)
            ?? throw new InvalidOperationException("Você não está matriculado neste curso.");

        var totalLessons = await _unitOfWork.Progresses.CountTotalLessonsInCourseAsync(courseId);
        var completedLessons = await _unitOfWork.Progresses.CountCompletedAsync(enrollment.Id);

        var percentage = totalLessons == 0
            ? 0
            : Math.Round((decimal)completedLessons / totalLessons * 100, 2);

        var progresses = await _unitOfWork.Progresses.GetByEnrollmentIdAsync(enrollment.Id);

        return new CourseProgressDto
        {
            EnrollmentId = enrollment.Id,
            CourseId = courseId,
            CourseTitle = course.Title,
            TotalLessons = totalLessons,
            CompletedLessons = completedLessons,
            CompletionPercentage = percentage,
            Progresses = _mapper.Map<IEnumerable<ProgressResponseDto>>(progresses)
        };
    }

    private async Task TryIssueCertificateAsync(Guid userId, Guid courseId, Guid enrollmentId)
    {
        var totalLessons = await _unitOfWork.Progresses.CountTotalLessonsInCourseAsync(courseId);
        if (totalLessons == 0) return;

        var completedLessons = await _unitOfWork.Progresses.CountCompletedAsync(enrollmentId);
        if (completedLessons < totalLessons) return;

        var alreadyIssued = await _unitOfWork.Certificates
            .GetByUserAndCourseAsync(userId, courseId);
        if (alreadyIssued is not null) return;

        var certificate = new Certificate(userId, courseId);
        await _unitOfWork.Certificates.AddAsync(certificate);
        await _unitOfWork.CommitAsync();
    }
}
