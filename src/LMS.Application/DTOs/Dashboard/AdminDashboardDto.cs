namespace LMS.Application.DTOs.Dashboard;

public class AdminDashboardDto
{
    public int TotalStudents { get; set; }
    public int TotalCourses { get; set; }
    public int TotalActiveCourses { get; set; }
    public int TotalEnrollments { get; set; }
    public int TotalCertificatesIssued { get; set; }
    public decimal AverageCompletionRate { get; set; }
    public IEnumerable<PopularCourseDto> MostPopularCourses { get; set; } = [];
}
