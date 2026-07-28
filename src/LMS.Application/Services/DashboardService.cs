using LMS.Application.DTOs.Dashboard;
using LMS.Application.Interfaces;

namespace LMS.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;
    private readonly ICurrentUserService _currentUser;

    public DashboardService(IDashboardRepository dashboardRepository, ICurrentUserService currentUser)
    {
        _dashboardRepository = dashboardRepository;
        _currentUser = currentUser;
    }

    public async Task<AdminDashboardDto> GetAdminDashboardAsync()
    {
        var totalStudents = await _dashboardRepository.CountStudentsAsync();
        var totalCourses = await _dashboardRepository.CountCoursesAsync();
        var totalActiveCourses = await _dashboardRepository.CountCoursesAsync(isActive: true);
        var totalEnrollments = await _dashboardRepository.CountEnrollmentsAsync();
        var totalCertificates = await _dashboardRepository.CountCertificatesAsync();
        var avgCompletion = await _dashboardRepository.GetAverageCompletionRateAsync();
        var popularCourses = await _dashboardRepository.GetMostPopularCoursesAsync(5);

        return new AdminDashboardDto
        {
            TotalStudents = totalStudents,
            TotalCourses = totalCourses,
            TotalActiveCourses = totalActiveCourses,
            TotalEnrollments = totalEnrollments,
            TotalCertificatesIssued = totalCertificates,
            AverageCompletionRate = avgCompletion,
            MostPopularCourses = popularCourses
        };
    }

    public async Task<StudentDashboardDto> GetStudentDashboardAsync()
    {
        var userId = _currentUser.UserId;

        var enrolledCourses = await _dashboardRepository.CountStudentEnrollmentsAsync(userId);
        var certificates = await _dashboardRepository.CountStudentCertificatesAsync(userId);
        var coursesProgress = await _dashboardRepository.GetStudentCoursesProgressAsync(userId);

        var completedCourses = coursesProgress.Count(x => x.CompletionPercentage == 100);
        var avgProgress = coursesProgress.Any()
            ? Math.Round(coursesProgress.Average(x => x.CompletionPercentage), 2)
            : 0;

        return new StudentDashboardDto
        {
            EnrolledCourses = enrolledCourses,
            CompletedCourses = completedCourses,
            CertificatesEarned = certificates,
            AverageProgress = avgProgress,
            CoursesInProgress = coursesProgress
        };
    }
}
