using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using LMS.Application.DTOs.Auth;
using LMS.Application.DTOs.Courses;
using LMS.IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.IntegrationTests.Controllers;

public class CoursesControllerTests : IClassFixture<LmsWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly LmsWebApplicationFactory _factory;

    public CoursesControllerTests(LmsWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var email = $"admin_{Guid.NewGuid()}@teste.com";
        var password = "Admin@123";

        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", new RegisterRequestDto
        {
            Name = "Admin Teste",
            Email = email,
            Password = password,
            ConfirmPassword = password
        });

        registerResponse.EnsureSuccessStatusCode();

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider
            .GetRequiredService<LMS.Infrastructure.Persistence.Context.LmsDbContext>();

        var user = db.Users.FirstOrDefault(x => x.Email == email);
        if (user == null) throw new Exception($"Usuário {email} não encontrado no banco.");

        db.Entry(user).Property("Role").CurrentValue = LMS.Domain.Enums.UserRole.Admin;
        await db.SaveChangesAsync();

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login",
            new LoginRequestDto { Email = email, Password = password });
        loginResponse.EnsureSuccessStatusCode();

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
        return loginResult!.AccessToken;
    }

    [Fact]
    public async Task GetAll_ShouldReturn200_WhenAuthenticated()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/courses");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_ShouldReturn401_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/courses");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_ShouldReturn201_WhenAdminAndValidRequest()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var request = new CourseRequestDto
        {
            Title = $"Curso Teste {Guid.NewGuid()}",
            Description = "Descricao completa do curso de teste para integracao."
        };

        var response = await _client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CourseResponseDto>();
        result!.Title.Should().Be(request.Title);
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldReturn403_WhenStudent()
    {
        var email = $"student_{Guid.NewGuid()}@teste.com";
        var password = "Teste@123";

        var client = _factory.CreateClient();

        await client.PostAsJsonAsync("/api/auth/register", new RegisterRequestDto
        {
            Name = "Student Teste",
            Email = email,
            Password = password,
            ConfirmPassword = password
        });

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login",
            new LoginRequestDto { Email = email, Password = password });
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginResult!.AccessToken);

        var request = new CourseRequestDto
        {
            Title = "Curso Proibido",
            Description = "Student nao pode criar curso."
        };

        var response = await client.PostAsJsonAsync("/api/courses", request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
