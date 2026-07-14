using FluentAssertions;
using LMS.Domain.Entities;

namespace LMS.UnitTests.Domain.Entities;

public class CertificateTests
{
    [Fact]
    public void Constructor_ShouldSetUserAndCourseIds()
    {
        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        var certificate = new Certificate(userId, courseId);

        certificate.UserId.Should().Be(userId);
        certificate.CourseId.Should().Be(courseId);
    }

    [Fact]
    public void Constructor_ShouldSetIssuedAtToUtcNow()
    {
        var before = DateTime.UtcNow;

        var certificate = new Certificate(Guid.NewGuid(), Guid.NewGuid());

        certificate.IssuedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(DateTime.UtcNow);
    }

    [Fact]
    public void Constructor_ShouldGenerateUppercaseCodeWithoutDashes()
    {
        var certificate = new Certificate(Guid.NewGuid(), Guid.NewGuid());

        certificate.Code.Should().NotBeNullOrEmpty();
        certificate.Code.Should().HaveLength(32);
        certificate.Code.Should().NotContain("-");
        certificate.Code.Should().Be(certificate.Code.ToUpperInvariant());
    }

    [Fact]
    public void Constructor_ShouldGenerateUniqueCodes()
    {
        var first = new Certificate(Guid.NewGuid(), Guid.NewGuid());
        var second = new Certificate(Guid.NewGuid(), Guid.NewGuid());

        first.Code.Should().NotBe(second.Code);
    }
}
