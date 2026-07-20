using AutoMapper;
using LMS.Application.DTOs.Modules;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;

namespace LMS.Application.Services;

public class ModuleService : IModuleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ModuleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ModuleResponseDto>> GetByCourseIdAsync(Guid courseId)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId)
            ?? throw new KeyNotFoundException($"Curso com Id '{courseId}' não encontrado.");

        var modules = await _unitOfWork.Modules.GetByCourseIdAsync(courseId);
        return _mapper.Map<IEnumerable<ModuleResponseDto>>(modules);
    }

    public async Task<ModuleWithLessonsDto> GetByIdWithLessonsAsync(Guid courseId, Guid moduleId)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId)
            ?? throw new KeyNotFoundException($"Curso com Id '{courseId}' não encontrado.");

        var module = await _unitOfWork.Modules.GetByIdWithLessonsAsync(moduleId)
            ?? throw new KeyNotFoundException($"Módulo com Id '{moduleId}' não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("O módulo não pertence ao curso informado.");

        return _mapper.Map<ModuleWithLessonsDto>(module);
    }

    public async Task<ModuleResponseDto> CreateAsync(Guid courseId, ModuleRequestDto request)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId)
            ?? throw new KeyNotFoundException($"Curso com Id '{courseId}' não encontrado.");

        var orderExists = await _unitOfWork.Modules.ExistsByOrderAsync(courseId, request.Order);
        if (orderExists)
            throw new InvalidOperationException($"Já existe um módulo com a ordem {request.Order} neste curso.");

        var module = new Module(request.Title, request.Order, courseId);

        await _unitOfWork.Modules.AddAsync(module);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ModuleResponseDto>(module);
    }

    public async Task<ModuleResponseDto> UpdateAsync(Guid courseId, Guid moduleId, ModuleRequestDto request)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId)
            ?? throw new KeyNotFoundException($"Curso com Id '{courseId}' não encontrado.");

        var module = await _unitOfWork.Modules.GetByIdAsync(moduleId)
            ?? throw new KeyNotFoundException($"Módulo com Id '{moduleId}' não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("O módulo não pertence ao curso informado.");

        var orderExists = await _unitOfWork.Modules.ExistsByOrderAsync(courseId, request.Order, moduleId);
        if (orderExists)
            throw new InvalidOperationException($"Já existe um módulo com a ordem {request.Order} neste curso.");

        module.Update(request.Title, request.Order);
        _unitOfWork.Modules.Update(module);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ModuleResponseDto>(module);
    }

    public async Task DeleteAsync(Guid courseId, Guid moduleId)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId)
            ?? throw new KeyNotFoundException($"Curso com Id '{courseId}' não encontrado.");

        var module = await _unitOfWork.Modules.GetByIdAsync(moduleId)
            ?? throw new KeyNotFoundException($"Módulo com Id '{moduleId}' não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("O módulo não pertence ao curso informado.");

        _unitOfWork.Modules.Delete(module);
        await _unitOfWork.CommitAsync();
    }
}
