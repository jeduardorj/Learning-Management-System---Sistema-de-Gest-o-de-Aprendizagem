using AutoMapper;
using LMS.Application.DTOs.Enrollments;
using LMS.Domain.Entities;

namespace LMS.Application.Mappings;

public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
        CreateMap<Enrollment, EnrollmentResponseDto>();

        CreateMap<Enrollment, EnrollmentWithCourseDto>()
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(dest => dest.CourseDescription, opt => opt.MapFrom(src => src.Course.Description));
    }
}
