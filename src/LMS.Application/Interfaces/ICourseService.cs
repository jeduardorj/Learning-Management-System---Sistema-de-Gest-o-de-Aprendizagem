using LMS.Application.Common;
using LMS.Application.DTOs.Courses;

namespace LMS.Application.Interfaces;

public interface ICourseService
{
    Task<PagedResult<CourseResponseDto>> GetAllAsync(CourseFilterDto filter);
    Task<CourseResponseDto> GetByIdAsync(Guid id);
    Task<CourseResponseDto> CreateAsync(CourseRequestDto request);
    Task<CourseResponseDto> UpdateAsync(Guid id, CourseRequestDto request);
    Task DeleteAsync(Guid id);
    Task<CourseResponseDto> ActivateAsync(Guid id);
    Task<CourseResponseDto> DeactivateAsync(Guid id);
}
