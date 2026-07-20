using AutoMapper;
using LMS.Application.DTOs.Lessons;
using LMS.Domain.Entities;

namespace LMS.Application.Mappings;

public class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<Lesson, LessonResponseDto>();
    }
}
