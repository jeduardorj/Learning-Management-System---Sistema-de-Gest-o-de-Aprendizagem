using AutoMapper;
using LMS.Application.DTOs.Courses;
using LMS.Domain.Entities;

namespace LMS.Application.Mappings;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseResponseDto>();
    }
}
