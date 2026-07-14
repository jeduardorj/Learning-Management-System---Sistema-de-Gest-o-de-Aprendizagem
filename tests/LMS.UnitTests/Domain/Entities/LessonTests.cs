using FluentAssertions;
using LMS.Domain.Entities;

namespace LMS.UnitTests.Domain.Entities;

public class LessonTests
{
    [Fact]
    public void Constructor_ShouldSetProvidedValues()
    {
        var moduleId = Guid.NewGuid();

        var lesson = new Lesson("Variables", "Content about variables", 1, moduleId);

        lesson.Title.Should().Be("Variables");
        lesson.Content.Should().Be("Content about variables");
        lesson.Order.Should().Be(1);
        lesson.ModuleId.Should().Be(moduleId);
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyProgresses()
    {
        var lesson = new Lesson("Variables", "Content", 1, Guid.NewGuid());

        lesson.Progresses.Should().BeEmpty();
    }

    [Fact]
    public void Update_ShouldChangeTitleContentOrderAndMarkUpdated()
    {
        var lesson = new Lesson("Variables", "Content", 1, Guid.NewGuid());

        lesson.Update("Data Types", "New content", 2);

        lesson.Title.Should().Be("Data Types");
        lesson.Content.Should().Be("New content");
        lesson.Order.Should().Be(2);
        lesson.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Update_ShouldNotChangeModuleId()
    {
        var moduleId = Guid.NewGuid();
        var lesson = new Lesson("Variables", "Content", 1, moduleId);

        lesson.Update("Data Types", "New content", 2);

        lesson.ModuleId.Should().Be(moduleId);
    }
}
