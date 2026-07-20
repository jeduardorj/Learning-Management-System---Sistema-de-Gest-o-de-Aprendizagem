using AutoMapper;
using LMS.Application.DTOs.Modules;
using LMS.Domain.Entities;

namespace LMS.Application.Mappings;

public class ModuleProfile : Profile
{
    public ModuleProfile()
    {
        CreateMap<Module, ModuleResponseDto>();
        CreateMap<Module, ModuleWithLessonsDto>();
    }
}
