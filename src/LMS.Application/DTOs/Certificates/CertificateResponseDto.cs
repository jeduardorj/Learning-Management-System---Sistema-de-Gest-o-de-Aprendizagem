namespace LMS.Application.DTOs.Certificates;

public class CertificateResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
}
