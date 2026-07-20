using LMS.Application.DTOs.Lessons;
using LMS.Application.Interfaces;
using LMS.Application.Validators.Lessons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/courses/{courseId:guid}/modules/{moduleId:guid}/lessons")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid courseId, Guid moduleId)
    {
        var result = await _lessonService.GetByModuleIdAsync(courseId, moduleId);
        return Ok(result);
    }

    [HttpGet("{lessonId:guid}")]
    public async Task<IActionResult> GetById(Guid courseId, Guid moduleId, Guid lessonId)
    {
        var result = await _lessonService.GetByIdAsync(courseId, moduleId, lessonId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Guid courseId, Guid moduleId, [FromBody] LessonRequestDto request)
    {
        var validator = new LessonRequestValidator();
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(x => x.ErrorMessage));

        var result = await _lessonService.CreateAsync(courseId, moduleId, request);
        return CreatedAtAction(nameof(GetById), new { courseId, moduleId, lessonId = result.Id }, result);
    }

    [HttpPut("{lessonId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid courseId, Guid moduleId, Guid lessonId, [FromBody] LessonRequestDto request)
    {
        var validator = new LessonRequestValidator();
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(x => x.ErrorMessage));

        var result = await _lessonService.UpdateAsync(courseId, moduleId, lessonId, request);
        return Ok(result);
    }

    [HttpDelete("{lessonId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid courseId, Guid moduleId, Guid lessonId)
    {
        await _lessonService.DeleteAsync(courseId, moduleId, lessonId);
        return NoContent();
    }
}
