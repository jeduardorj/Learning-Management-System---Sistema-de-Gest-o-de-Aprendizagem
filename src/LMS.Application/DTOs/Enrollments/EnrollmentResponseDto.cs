namespace LMS.Application.DTOs.Enrollments;

public class EnrollmentResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime EnrolledAt { get; set; }
}
