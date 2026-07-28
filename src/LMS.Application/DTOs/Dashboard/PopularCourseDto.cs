namespace LMS.Application.DTOs.Dashboard;

public class PopularCourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int EnrollmentCount { get; set; }
    public decimal AverageCompletionPercentage { get; set; }
}
