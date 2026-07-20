using LMS.Application.DTOs.Modules;

namespace LMS.Application.Interfaces;

public interface IModuleService
{
    Task<IEnumerable<ModuleResponseDto>> GetByCourseIdAsync(Guid courseId);
    Task<ModuleWithLessonsDto> GetByIdWithLessonsAsync(Guid courseId, Guid moduleId);
    Task<ModuleResponseDto> CreateAsync(Guid courseId, ModuleRequestDto request);
    Task<ModuleResponseDto> UpdateAsync(Guid courseId, Guid moduleId, ModuleRequestDto request);
    Task DeleteAsync(Guid courseId, Guid moduleId);
}
