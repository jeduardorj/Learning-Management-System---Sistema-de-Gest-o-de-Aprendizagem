using AutoMapper;
using LMS.Application.Common;
using LMS.Application.DTOs.Courses;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<CourseResponseDto>> GetAllAsync(CourseFilterDto filter)
    {
        var query = _unitOfWork.Courses.GetQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(x => x.Title.Contains(filter.Search) ||
                                     x.Description.Contains(filter.Search));

        if (filter.IsActive.HasValue)
            query = query.Where(x => x.IsActive == filter.IsActive.Value);

        var totalItems = await query.CountAsync();

        var courses = await query
            .OrderBy(x => x.Title)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<CourseResponseDto>>(courses);

        return PagedResult<CourseResponseDto>.Create(dtos, totalItems, filter.Page, filter.PageSize);
    }

    public async Task<CourseResponseDto> GetByIdAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Curso com Id '{id}' não encontrado.");

        return _mapper.Map<CourseResponseDto>(course);
    }

    public async Task<CourseResponseDto> CreateAsync(CourseRequestDto request)
    {
        var titleExists = await _unitOfWork.Courses.ExistsByTitleAsync(request.Title);
        if (titleExists)
            throw new InvalidOperationException($"Já existe um curso com o título '{request.Title}'.");

        var course = new Course(request.Title, request.Description);

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CourseResponseDto>(course);
    }

    public async Task<CourseResponseDto> UpdateAsync(Guid id, CourseRequestDto request)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Curso com Id '{id}' não encontrado.");

        var titleExists = await _unitOfWork.Courses.ExistsByTitleAsync(request.Title);
        if (titleExists && course.Title != request.Title)
            throw new InvalidOperationException($"Já existe um curso com o título '{request.Title}'.");

        course.Update(request.Title, request.Description);

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CourseResponseDto>(course);
    }

    public async Task DeleteAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Curso com Id '{id}' não encontrado.");

        _unitOfWork.Courses.Delete(course);
        await _unitOfWork.CommitAsync();
    }

    public async Task<CourseResponseDto> ActivateAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Curso com Id '{id}' não encontrado.");

        course.Activate();
        _unitOfWork.Courses.Update(course);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CourseResponseDto>(course);
    }

    public async Task<CourseResponseDto> DeactivateAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Curso com Id '{id}' não encontrado.");

        course.Deactivate();
        _unitOfWork.Courses.Update(course);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CourseResponseDto>(course);
    }
}
