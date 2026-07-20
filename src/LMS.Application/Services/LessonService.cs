using AutoMapper;
using LMS.Application.DTOs.Lessons;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;

namespace LMS.Application.Services;

public class LessonService : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LessonService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LessonResponseDto>> GetByModuleIdAsync(Guid courseId, Guid moduleId)
    {
        var module = await _unitOfWork.Modules.GetByIdAsync(moduleId)
            ?? throw new KeyNotFoundException($"Módulo com Id '{moduleId}' não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("O módulo não pertence ao curso informado.");

        var lessons = await _unitOfWork.Lessons.GetByModuleIdAsync(moduleId);
        return _mapper.Map<IEnumerable<LessonResponseDto>>(lessons);
    }

    public async Task<LessonResponseDto> GetByIdAsync(Guid courseId, Guid moduleId, Guid lessonId)
    {
        var module = await _unitOfWork.Modules.GetByIdAsync(moduleId)
            ?? throw new KeyNotFoundException($"Módulo com Id '{moduleId}' não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("O módulo não pertence ao curso informado.");

        var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId)
            ?? throw new KeyNotFoundException($"Aula com Id '{lessonId}' não encontrada.");

        if (lesson.ModuleId != moduleId)
            throw new InvalidOperationException("A aula não pertence ao módulo informado.");

        return _mapper.Map<LessonResponseDto>(lesson);
    }

    public async Task<LessonResponseDto> CreateAsync(Guid courseId, Guid moduleId, LessonRequestDto request)
    {
        var module = await _unitOfWork.Modules.GetByIdAsync(moduleId)
            ?? throw new KeyNotFoundException($"Módulo com Id '{moduleId}' não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("O módulo não pertence ao curso informado.");

        var orderExists = await _unitOfWork.Lessons.ExistsByOrderAsync(moduleId, request.Order);
        if (orderExists)
            throw new InvalidOperationException($"Já existe uma aula com a ordem {request.Order} neste módulo.");

        var lesson = new Lesson(request.Title, request.Content, request.Order, moduleId);

        await _unitOfWork.Lessons.AddAsync(lesson);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<LessonResponseDto>(lesson);
    }

    public async Task<LessonResponseDto> UpdateAsync(Guid courseId, Guid moduleId, Guid lessonId, LessonRequestDto request)
    {
        var module = await _unitOfWork.Modules.GetByIdAsync(moduleId)
            ?? throw new KeyNotFoundException($"Módulo com Id '{moduleId}' não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("O módulo não pertence ao curso informado.");

        var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId)
            ?? throw new KeyNotFoundException($"Aula com Id '{lessonId}' não encontrada.");

        if (lesson.ModuleId != moduleId)
            throw new InvalidOperationException("A aula não pertence ao módulo informado.");

        var orderExists = await _unitOfWork.Lessons.ExistsByOrderAsync(moduleId, request.Order, lessonId);
        if (orderExists)
            throw new InvalidOperationException($"Já existe uma aula com a ordem {request.Order} neste módulo.");

        lesson.Update(request.Title, request.Content, request.Order);
        _unitOfWork.Lessons.Update(lesson);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<LessonResponseDto>(lesson);
    }

    public async Task DeleteAsync(Guid courseId, Guid moduleId, Guid lessonId)
    {
        var module = await _unitOfWork.Modules.GetByIdAsync(moduleId)
            ?? throw new KeyNotFoundException($"Módulo com Id '{moduleId}' não encontrado.");

        if (module.CourseId != courseId)
            throw new InvalidOperationException("O módulo não pertence ao curso informado.");

        var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId)
            ?? throw new KeyNotFoundException($"Aula com Id '{lessonId}' não encontrada.");

        if (lesson.ModuleId != moduleId)
            throw new InvalidOperationException("A aula não pertence ao módulo informado.");

        _unitOfWork.Lessons.Delete(lesson);
        await _unitOfWork.CommitAsync();
    }
}
