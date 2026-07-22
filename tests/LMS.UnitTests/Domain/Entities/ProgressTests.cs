using FluentAssertions;
using LMS.Domain.Entities;

namespace LMS.UnitTests.Domain.Entities;

public class ProgressTests
{
    [Fact]
    public void Constructor_ShouldSetEnrollmentAndLessonIds()
    {
        var enrollmentId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var progress = new Progress(enrollmentId, lessonId);

        progress.EnrollmentId.Should().Be(enrollmentId);
        progress.LessonId.Should().Be(lessonId);
    }

    [Fact]
    public void Constructor_ShouldNotBeCompleted()
    {
        var progress = new Progress(Guid.NewGuid(), Guid.NewGuid());

        progress.Completed.Should().BeFalse();
        progress.CompletedAt.Should().BeNull();
    }

    [Fact]
    public void Complete_ShouldSetCompletedAndCompletedAtAndMarkUpdated()
    {
        var progress = new Progress(Guid.NewGuid(), Guid.NewGuid());

        progress.Complete();

        progress.Completed.Should().BeTrue();
        progress.CompletedAt.Should().NotBeNull();
        progress.CompletedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        progress.UpdatedAt.Should().NotBeNull();
    }
}
