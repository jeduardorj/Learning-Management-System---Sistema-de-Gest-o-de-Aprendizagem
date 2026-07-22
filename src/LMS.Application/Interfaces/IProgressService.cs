using LMS.Application.DTOs.Progress;

namespace LMS.Application.Interfaces;

public interface IProgressService
{
    Task<ProgressResponseDto> MarkLessonAsCompletedAsync(Guid courseId, Guid lessonId);
    Task<CourseProgressDto> GetCourseProgressAsync(Guid courseId);
}
