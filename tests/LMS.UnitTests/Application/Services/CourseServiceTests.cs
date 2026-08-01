using AutoMapper;
using FluentAssertions;
using LMS.Application.DTOs.Courses;
using LMS.Application.Mappings;
using LMS.Application.Services;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace LMS.UnitTests.Application.Services;

public class CourseServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICourseRepository> _mockCourseRepository;
    private readonly IMapper _mapper;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCourseRepository = new Mock<ICourseRepository>();

        _mockUnitOfWork.Setup(x => x.Courses).Returns(_mockCourseRepository.Object);
        _mockUnitOfWork.Setup(x => x.CommitAsync()).ReturnsAsync(1);
        _mockCourseRepository
            .Setup(x => x.Delete(It.IsAny<Course>()))
            .Callback<Course>(c => c.MarkAsDeleted());

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CourseProfile).Assembly));
        var provider = services.BuildServiceProvider();
        _mapper = provider.GetRequiredService<IMapper>();

        _courseService = new CourseService(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCourse_WhenTitleDoesNotExist()
    {
        var request = new CourseRequestDto
        {
            Title = "C# Avancado",
            Description = "Curso completo de C# para desenvolvedores."
        };

        _mockCourseRepository
            .Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(false);

        _mockCourseRepository
            .Setup(x => x.AddAsync(It.IsAny<Course>()))
            .Returns(Task.CompletedTask);

        var result = await _courseService.CreateAsync(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
        result.IsActive.Should().BeTrue();

        _mockCourseRepository.Verify(x => x.AddAsync(It.IsAny<Course>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenTitleAlreadyExists()
    {
        var request = new CourseRequestDto
        {
            Title = "C# Avancado",
            Description = "Descricao qualquer do curso."
        };

        _mockCourseRepository
            .Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(true);

        var act = async () => await _courseService.CreateAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{request.Title}*");

        _mockCourseRepository.Verify(x => x.AddAsync(It.IsAny<Course>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCourse_WhenExists()
    {
        var courseId = Guid.NewGuid();
        var course = new Course("C# Avancado", "Descricao do curso aqui.");

        _mockCourseRepository
            .Setup(x => x.GetByIdAsync(courseId))
            .ReturnsAsync(course);

        var result = await _courseService.GetByIdAsync(courseId);

        result.Should().NotBeNull();
        result.Title.Should().Be(course.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenNotExists()
    {
        var courseId = Guid.NewGuid();

        _mockCourseRepository
            .Setup(x => x.GetByIdAsync(courseId))
            .ReturnsAsync((Course?)null);

        var act = async () => await _courseService.GetByIdAsync(courseId);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{courseId}*");
    }

    [Fact]
    public async Task DeleteAsync_ShouldMarkAsDeleted_WhenCourseExists()
    {
        var courseId = Guid.NewGuid();
        var course = new Course("C# Avancado", "Descricao do curso aqui.");

        _mockCourseRepository
            .Setup(x => x.GetByIdAsync(courseId))
            .ReturnsAsync(course);

        await _courseService.DeleteAsync(courseId);

        course.IsDeleted.Should().BeTrue();
        _mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCourse_WhenValidRequest()
    {
        var courseId = Guid.NewGuid();
        var course = new Course("Titulo Original", "Descricao original do curso.");
        var request = new CourseRequestDto
        {
            Title = "Titulo Atualizado",
            Description = "Descricao atualizada do curso."
        };

        _mockCourseRepository
            .Setup(x => x.GetByIdAsync(courseId))
            .ReturnsAsync(course);

        _mockCourseRepository
            .Setup(x => x.ExistsByTitleAsync(request.Title))
            .ReturnsAsync(false);

        var result = await _courseService.UpdateAsync(courseId, request);

        result.Title.Should().Be(request.Title);
        result.Description.Should().Be(request.Description);
        _mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }
}
