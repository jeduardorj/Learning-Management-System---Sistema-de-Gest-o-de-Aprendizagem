using FluentAssertions;
using LMS.Domain.Entities;

namespace LMS.UnitTests.Domain.Entities;

public class BaseEntityTests
{
    private sealed class FakeEntity : BaseEntity
    {
        public FakeEntity() { }
    }

    [Fact]
    public void Constructor_ShouldInitializeIdWithNonEmptyGuid()
    {
        var entity = new FakeEntity();

        entity.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Constructor_ShouldSetCreatedAtToUtcNow()
    {
        var before = DateTime.UtcNow;

        var entity = new FakeEntity();

        var after = DateTime.UtcNow;
        entity.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Fact]
    public void Constructor_ShouldNotSetUpdatedAt()
    {
        var entity = new FakeEntity();

        entity.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldNotMarkAsDeleted()
    {
        var entity = new FakeEntity();

        entity.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void NewEntities_ShouldHaveUniqueIds()
    {
        var first = new FakeEntity();
        var second = new FakeEntity();

        first.Id.Should().NotBe(second.Id);
    }

    [Fact]
    public void MarkAsDeleted_ShouldSetIsDeletedAndUpdatedAt()
    {
        var entity = new FakeEntity();

        entity.MarkAsDeleted();

        entity.IsDeleted.Should().BeTrue();
        entity.UpdatedAt.Should().NotBeNull();
        entity.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MarkAsUpdated_ShouldSetUpdatedAtWithoutDeleting()
    {
        var entity = new FakeEntity();

        entity.MarkAsUpdated();

        entity.IsDeleted.Should().BeFalse();
        entity.UpdatedAt.Should().NotBeNull();
        entity.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
