namespace LMS.Domain.Entities;

public class Enrollment : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid CourseId { get; private set; }
    public DateTime EnrolledAt { get; private set; }

    public User User { get; private set; } = null!;
    public Course Course { get; private set; } = null!;
    public ICollection<Progress> Progresses { get; private set; } = [];

    protected Enrollment() { }

    public Enrollment(Guid userId, Guid courseId)
    {
        UserId = userId;
        CourseId = courseId;
        EnrolledAt = DateTime.UtcNow;
    }
}
