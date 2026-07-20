using LMS.Application.DTOs.Modules;
using LMS.Application.Interfaces;
using LMS.Application.Validators.Modules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/courses/{courseId:guid}/modules")]
[Authorize]
public class ModulesController : ControllerBase
{
    private readonly IModuleService _moduleService;

    public ModulesController(IModuleService moduleService)
    {
        _moduleService = moduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid courseId)
    {
        var result = await _moduleService.GetByCourseIdAsync(courseId);
        return Ok(result);
    }

    [HttpGet("{moduleId:guid}")]
    public async Task<IActionResult> GetById(Guid courseId, Guid moduleId)
    {
        var result = await _moduleService.GetByIdWithLessonsAsync(courseId, moduleId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Guid courseId, [FromBody] ModuleRequestDto request)
    {
        var validator = new ModuleRequestValidator();
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(x => x.ErrorMessage));

        var result = await _moduleService.CreateAsync(courseId, request);
        return CreatedAtAction(nameof(GetById), new { courseId, moduleId = result.Id }, result);
    }

    [HttpPut("{moduleId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid courseId, Guid moduleId, [FromBody] ModuleRequestDto request)
    {
        var validator = new ModuleRequestValidator();
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(x => x.ErrorMessage));

        var result = await _moduleService.UpdateAsync(courseId, moduleId, request);
        return Ok(result);
    }

    [HttpDelete("{moduleId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid courseId, Guid moduleId)
    {
        await _moduleService.DeleteAsync(courseId, moduleId);
        return NoContent();
    }
}
