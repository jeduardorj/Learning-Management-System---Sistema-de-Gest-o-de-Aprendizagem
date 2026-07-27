using AutoMapper;
using LMS.Application.DTOs.Certificates;
using LMS.Application.Interfaces;
using LMS.Domain.Interfaces.Repositories;

namespace LMS.Application.Services;

public class CertificateService : ICertificateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public CertificateService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CertificateResponseDto?> GetMyCertificateAsync(Guid courseId)
    {
        var userId = _currentUser.UserId;

        var certificate = await _unitOfWork.Certificates.GetByUserAndCourseAsync(userId, courseId);
        if (certificate is null) return null;

        var full = await _unitOfWork.Certificates.GetByCodeAsync(certificate.Code);
        return _mapper.Map<CertificateResponseDto>(full);
    }

    public async Task<IEnumerable<CertificateResponseDto>> GetMyCertificatesAsync()
    {
        var userId = _currentUser.UserId;
        var certificates = await _unitOfWork.Certificates.GetByUserIdAsync(userId);

        var result = new List<CertificateResponseDto>();
        foreach (var cert in certificates)
        {
            var full = await _unitOfWork.Certificates.GetByCodeAsync(cert.Code);
            if (full is not null)
                result.Add(_mapper.Map<CertificateResponseDto>(full));
        }

        return result;
    }

    public async Task<CertificateValidationDto> ValidateByCodeAsync(string code)
    {
        var certificate = await _unitOfWork.Certificates.GetByCodeAsync(code);

        if (certificate is null)
            return new CertificateValidationDto { IsValid = false, Code = code };

        return _mapper.Map<CertificateValidationDto>(certificate);
    }
}
