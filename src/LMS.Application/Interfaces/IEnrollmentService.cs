using LMS.Application.DTOs.Enrollments;

namespace LMS.Application.Interfaces;

public interface IEnrollmentService
{
    Task<EnrollmentResponseDto> EnrollAsync(Guid courseId);
    Task<IEnumerable<EnrollmentWithCourseDto>> GetMyEnrollmentsAsync();
    Task<IEnumerable<EnrollmentResponseDto>> GetByCourseIdAsync(Guid courseId);
    Task UnenrollAsync(Guid courseId);
}
