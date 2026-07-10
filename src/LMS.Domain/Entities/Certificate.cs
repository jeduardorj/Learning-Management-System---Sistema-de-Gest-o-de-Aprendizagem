namespace LMS.Domain.Entities;

public class Certificate : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid CourseId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public DateTime IssuedAt { get; private set; }

    public User User { get; private set; } = null!;
    public Course Course { get; private set; } = null!;

    protected Certificate() { }

    public Certificate(Guid userId, Guid courseId)
    {
        UserId = userId;
        CourseId = courseId;
        Code = Guid.NewGuid().ToString("N").ToUpper();
        IssuedAt = DateTime.UtcNow;
    }
}
