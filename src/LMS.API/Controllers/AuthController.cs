using LMS.Application.DTOs.Auth;
using LMS.Application.Interfaces;
using LMS.Application.Validators.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var validator = new RegisterRequestValidator();
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(x => x.ErrorMessage));

        var result = await _authService.RegisterAsync(request);
        return Created(string.Empty, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var validator = new LoginRequestValidator();
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(x => x.ErrorMessage));

        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return Ok(result);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequestDto request)
    {
        await _authService.RevokeTokenAsync(request.RefreshToken);
        return NoContent();
    }
}
