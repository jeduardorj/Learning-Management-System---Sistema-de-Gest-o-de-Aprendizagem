using FluentAssertions;
using LMS.Domain.Entities;

namespace LMS.UnitTests.Domain.Entities;

public class CourseTests
{
    [Fact]
    public void Constructor_ShouldSetTitleAndDescription()
    {
        var course = new Course("C# Basics", "Intro to C#");

        course.Title.Should().Be("C# Basics");
        course.Description.Should().Be("Intro to C#");
    }

    [Fact]
    public void Constructor_ShouldBeActiveByDefault()
    {
        var course = new Course("C# Basics", "Intro to C#");

        course.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyCollections()
    {
        var course = new Course("C# Basics", "Intro to C#");

        course.Modules.Should().BeEmpty();
        course.Enrollments.Should().BeEmpty();
        course.Certificates.Should().BeEmpty();
    }

    [Fact]
    public void Update_ShouldChangeTitleAndDescriptionAndMarkUpdated()
    {
        var course = new Course("Old", "Old desc");

        course.Update("New", "New desc");

        course.Title.Should().Be("New");
        course.Description.Should().Be("New desc");
        course.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalseAndMarkUpdated()
    {
        var course = new Course("C# Basics", "Intro to C#");

        course.Deactivate();

        course.IsActive.Should().BeFalse();
        course.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Activate_ShouldSetIsActiveTrueAndMarkUpdated()
    {
        var course = new Course("C# Basics", "Intro to C#");
        course.Deactivate();

        course.Activate();

        course.IsActive.Should().BeTrue();
        course.UpdatedAt.Should().NotBeNull();
    }
}
