using FluentAssertions;
using LMS.Domain.Entities;

namespace LMS.UnitTests.Domain.Entities;

public class EnrollmentTests
{
    [Fact]
    public void Constructor_ShouldSetUserAndCourseIds()
    {
        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        var enrollment = new Enrollment(userId, courseId);

        enrollment.UserId.Should().Be(userId);
        enrollment.CourseId.Should().Be(courseId);
    }

    [Fact]
    public void Constructor_ShouldSetEnrolledAtToUtcNow()
    {
        var before = DateTime.UtcNow;

        var enrollment = new Enrollment(Guid.NewGuid(), Guid.NewGuid());

        enrollment.EnrolledAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(DateTime.UtcNow);
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyProgresses()
    {
        var enrollment = new Enrollment(Guid.NewGuid(), Guid.NewGuid());

        enrollment.Progresses.Should().BeEmpty();
    }
}
