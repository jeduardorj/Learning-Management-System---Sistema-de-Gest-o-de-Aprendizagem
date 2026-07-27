using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificatesController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    /// <summary>
    /// Aluno consulta seu certificado de um curso específico
    /// </summary>
    [HttpGet("my/{courseId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetMyCertificate(Guid courseId)
    {
        var result = await _certificateService.GetMyCertificateAsync(courseId);
        if (result is null)
            return NotFound("Certificado não encontrado. Conclua 100% do curso para receber o certificado.");

        return Ok(result);
    }

    /// <summary>
    /// Aluno lista todos os seus certificados
    /// </summary>
    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyCertificates()
    {
        var result = await _certificateService.GetMyCertificatesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Validação pública de certificado por código — sem autenticação
    /// </summary>
    [HttpGet("validate/{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> Validate(string code)
    {
        var result = await _certificateService.ValidateByCodeAsync(code);

        if (!result.IsValid)
            return NotFound(new { IsValid = false, Message = "Certificado não encontrado." });

        return Ok(result);
    }
}
