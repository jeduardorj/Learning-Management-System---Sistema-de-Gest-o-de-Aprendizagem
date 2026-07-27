using AutoMapper;
using LMS.Application.DTOs.Certificates;
using LMS.Domain.Entities;

namespace LMS.Application.Mappings;

public class CertificateProfile : Profile
{
    public CertificateProfile()
    {
        CreateMap<Certificate, CertificateResponseDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title));

        CreateMap<Certificate, CertificateValidationDto>()
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title));
    }
}
