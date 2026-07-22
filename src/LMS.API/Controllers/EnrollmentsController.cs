using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    /// <summary>
    /// Aluno se matricula em um curso
    /// </summary>
    [HttpPost("{courseId:guid}")]
    public async Task<IActionResult> Enroll(Guid courseId)
    {
        var result = await _enrollmentService.EnrollAsync(courseId);
        return Created(string.Empty, result);
    }

    /// <summary>
    /// Aluno vê suas próprias matrículas
    /// </summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyEnrollments()
    {
        var result = await _enrollmentService.GetMyEnrollmentsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Admin vê todos os alunos matriculados em um curso
    /// </summary>
    [HttpGet("course/{courseId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByCourse(Guid courseId)
    {
        var result = await _enrollmentService.GetByCourseIdAsync(courseId);
        return Ok(result);
    }

    /// <summary>
    /// Aluno cancela sua matrícula
    /// </summary>
    [HttpDelete("{courseId:guid}")]
    public async Task<IActionResult> Unenroll(Guid courseId)
    {
        await _enrollmentService.UnenrollAsync(courseId);
        return NoContent();
    }
}
