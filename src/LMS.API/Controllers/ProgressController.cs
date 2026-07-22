using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/courses/{courseId:guid}/progress")]
[Authorize]
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;

    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    /// <summary>
    /// Marca uma aula como concluída
    /// </summary>
    [HttpPost("lessons/{lessonId:guid}")]
    public async Task<IActionResult> MarkAsCompleted(Guid courseId, Guid lessonId)
    {
        var result = await _progressService.MarkLessonAsCompletedAsync(courseId, lessonId);
        return Ok(result);
    }

    /// <summary>
    /// Retorna o progresso completo do aluno no curso
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProgress(Guid courseId)
    {
        var result = await _progressService.GetCourseProgressAsync(courseId);
        return Ok(result);
    }
}
