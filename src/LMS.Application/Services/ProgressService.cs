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

        // Verifica se o aluno está matriculado no curso
        var enrollment = await _unitOfWork.Enrollments.GetByUserAndCourseAsync(userId, courseId)
            ?? throw new InvalidOperationException("Você não está matriculado neste curso.");

        // Verifica se a aula existe e pertence ao curso
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId)
            ?? throw new KeyNotFoundException($"Aula com Id '{lessonId}' não encontrada.");

        var module = await _unitOfWork.Modules.GetByIdAsync(lesson.ModuleId)
            ?? throw new KeyNotFoundException("Módulo não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("A aula não pertence a este curso.");

        // Idempotência — se já existe, retorna sem criar duplicata
        var existing = await _unitOfWork.Progresses
            .GetByEnrollmentAndLessonAsync(enrollment.Id, lessonId);

        if (existing is not null)
        {
            if (!existing.Completed)
            {
                existing.Complete();
                _unitOfWork.Progresses.Update(existing);
                await _unitOfWork.CommitAsync();
            }

            return _mapper.Map<ProgressResponseDto>(existing);
        }

        // Cria novo registro de progresso já marcado como concluído
        var progress = new Progress(enrollment.Id, lessonId);
        progress.Complete();

        await _unitOfWork.Progresses.AddAsync(progress);
        await _unitOfWork.CommitAsync();

        // Carrega a aula para o mapeamento do título
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
}
