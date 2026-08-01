using FluentAssertions;
using LMS.Domain.Entities;

namespace LMS.UnitTests.Domain.Entities;

public class CourseTests
{
    [Fact]
    public void Constructor_ShouldCreateCourse_WithCorrectValues()
    {
        // Arrange
        var title = "C# do Zero ao Avançado";
        var description = "Aprenda C# desde os fundamentos.";

        // Act
        var course = new Course(title, description);

        // Assert
        course.Title.Should().Be(title);
        course.Description.Should().Be(description);
        course.IsActive.Should().BeTrue();
        course.IsDeleted.Should().BeFalse();
        course.Id.Should().NotBeEmpty();
        course.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Deactivate_ShouldSetIsActive_ToFalse()
    {
        // Arrange
        var course = new Course("Título", "Descrição do curso aqui.");

        // Act
        course.Deactivate();

        // Assert
        course.IsActive.Should().BeFalse();
        course.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Activate_ShouldSetIsActive_ToTrue()
    {
        // Arrange
        var course = new Course("Título", "Descrição do curso aqui.");
        course.Deactivate();

        // Act
        course.Activate();

        // Assert
        course.IsActive.Should().BeTrue();
        course.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Update_ShouldChangeTitle_AndDescription()
    {
        // Arrange
        var course = new Course("Título Original", "Descrição original do curso.");
        var newTitle = "Novo Título";
        var newDescription = "Nova descrição do curso atualizado.";

        // Act
        course.Update(newTitle, newDescription);

        // Assert
        course.Title.Should().Be(newTitle);
        course.Description.Should().Be(newDescription);
        course.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void MarkAsDeleted_ShouldSetIsDeleted_ToTrue()
    {
        // Arrange
        var course = new Course("Título", "Descrição do curso aqui.");

        // Act
        course.MarkAsDeleted();

        // Assert
        course.IsDeleted.Should().BeTrue();
        course.UpdatedAt.Should().NotBeNull();
    }
}
