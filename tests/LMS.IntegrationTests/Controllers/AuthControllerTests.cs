using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LMS.Application.DTOs.Auth;
using LMS.IntegrationTests.Setup;

namespace LMS.IntegrationTests.Controllers;

public class AuthControllerTests : IClassFixture<LmsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(LmsWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ShouldReturn201_WhenValidRequest()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = "Teste Integration",
            Email = $"integration_{Guid.NewGuid()}@teste.com",
            Password = "Teste@123",
            ConfirmPassword = "Teste@123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        result.Should().NotBeNull();
        result!.AccessToken.Should().NotBeNullOrEmpty();
        result.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Register_ShouldReturn400_WhenPasswordIsWeak()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Name = "Teste",
            Email = "teste@teste.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturn200_WhenValidCredentials()
    {
        // Arrange — primeiro registra
        var email = $"login_{Guid.NewGuid()}@teste.com";
        var password = "Teste@123";

        await _client.PostAsJsonAsync("/api/auth/register", new RegisterRequestDto
        {
            Name = "Login Teste",
            Email = email,
            Password = password,
            ConfirmPassword = password
        });

        var loginRequest = new LoginRequestDto
        {
            Email = email,
            Password = password
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        result!.AccessToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_ShouldReturn401_WhenInvalidCredentials()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "naoexiste@teste.com",
            Password = "Senha@Errada123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
