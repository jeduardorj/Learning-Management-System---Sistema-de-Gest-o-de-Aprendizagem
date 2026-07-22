using AutoMapper;
using LMS.Application.DTOs.Progress;
using LMS.Domain.Entities;

namespace LMS.Application.Mappings;

public class ProgressProfile : Profile
{
    public ProgressProfile()
    {
        CreateMap<Progress, ProgressResponseDto>()
            .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson.Title));
    }
}
