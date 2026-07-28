using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Dashboard administrativo com indicadores gerais do sistema
    /// </summary>
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAdminDashboard()
    {
        var result = await _dashboardService.GetAdminDashboardAsync();
        return Ok(result);
    }

    /// <summary>
    /// Dashboard do aluno com seu progresso pessoal
    /// </summary>
    [HttpGet("student")]
    public async Task<IActionResult> GetStudentDashboard()
    {
        var result = await _dashboardService.GetStudentDashboardAsync();
        return Ok(result);
    }
}
