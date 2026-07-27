using LMS.Application.DTOs.Certificates;

namespace LMS.Application.Interfaces;

public interface ICertificateService
{
    Task<CertificateResponseDto?> GetMyCertificateAsync(Guid courseId);
    Task<IEnumerable<CertificateResponseDto>> GetMyCertificatesAsync();
    Task<CertificateValidationDto> ValidateByCodeAsync(string code);
}
