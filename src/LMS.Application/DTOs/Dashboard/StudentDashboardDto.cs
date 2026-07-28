namespace LMS.Application.DTOs.Dashboard;

public class StudentDashboardDto
{
    public int EnrolledCourses { get; set; }
    public int CompletedCourses { get; set; }
    public int CertificatesEarned { get; set; }
    public decimal AverageProgress { get; set; }
    public IEnumerable<StudentCourseProgressDto> CoursesInProgress { get; set; } = [];
}

public class StudentCourseProgressDto
{
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public decimal CompletionPercentage { get; set; }
    public bool HasCertificate { get; set; }
}
