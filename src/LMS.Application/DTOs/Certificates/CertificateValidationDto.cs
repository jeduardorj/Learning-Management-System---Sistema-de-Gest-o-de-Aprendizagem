namespace LMS.Application.DTOs.Certificates;

public class CertificateValidationDto
{
    public bool IsValid { get; set; }
    public string Code { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
}
