using LMS.Application.DTOs.Dashboard;

namespace LMS.Application.Interfaces;

public interface IDashboardRepository
{
    Task<int> CountStudentsAsync();
    Task<int> CountCoursesAsync(bool? isActive = null);
    Task<int> CountEnrollmentsAsync();
    Task<int> CountCertificatesAsync();
    Task<decimal> GetAverageCompletionRateAsync();
    Task<IEnumerable<PopularCourseDto>> GetMostPopularCoursesAsync(int top = 5);
    Task<int> CountStudentEnrollmentsAsync(Guid userId);
    Task<int> CountStudentCertificatesAsync(Guid userId);
    Task<IEnumerable<StudentCourseProgressDto>> GetStudentCoursesProgressAsync(Guid userId);
}
