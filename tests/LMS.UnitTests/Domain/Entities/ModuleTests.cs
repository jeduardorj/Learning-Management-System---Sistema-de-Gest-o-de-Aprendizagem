using FluentAssertions;
using LMS.Domain.Entities;

namespace LMS.UnitTests.Domain.Entities;

public class ModuleTests
{
    [Fact]
    public void Constructor_ShouldSetProvidedValues()
    {
        var courseId = Guid.NewGuid();

        var module = new Module("Introduction", 1, courseId);

        module.Title.Should().Be("Introduction");
        module.Order.Should().Be(1);
        module.CourseId.Should().Be(courseId);
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyLessons()
    {
        var module = new Module("Introduction", 1, Guid.NewGuid());

        module.Lessons.Should().BeEmpty();
    }

    [Fact]
    public void Update_ShouldChangeTitleAndOrderAndMarkUpdated()
    {
        var module = new Module("Introduction", 1, Guid.NewGuid());

        module.Update("Getting Started", 2);

        module.Title.Should().Be("Getting Started");
        module.Order.Should().Be(2);
        module.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Update_ShouldNotChangeCourseId()
    {
        var courseId = Guid.NewGuid();
        var module = new Module("Introduction", 1, courseId);

        module.Update("Getting Started", 2);

        module.CourseId.Should().Be(courseId);
    }
}
