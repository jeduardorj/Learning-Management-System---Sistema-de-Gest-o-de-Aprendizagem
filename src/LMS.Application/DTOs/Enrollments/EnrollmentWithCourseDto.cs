namespace LMS.Application.DTOs.Enrollments;

public class EnrollmentWithCourseDto
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string CourseDescription { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
}
