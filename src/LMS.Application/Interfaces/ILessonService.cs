using LMS.Application.DTOs.Lessons;

namespace LMS.Application.Interfaces;

public interface ILessonService
{
    Task<IEnumerable<LessonResponseDto>> GetByModuleIdAsync(Guid courseId, Guid moduleId);
    Task<LessonResponseDto> GetByIdAsync(Guid courseId, Guid moduleId, Guid lessonId);
    Task<LessonResponseDto> CreateAsync(Guid courseId, Guid moduleId, LessonRequestDto request);
    Task<LessonResponseDto> UpdateAsync(Guid courseId, Guid moduleId, Guid lessonId, LessonRequestDto request);
    Task DeleteAsync(Guid courseId, Guid moduleId, Guid lessonId);
}
