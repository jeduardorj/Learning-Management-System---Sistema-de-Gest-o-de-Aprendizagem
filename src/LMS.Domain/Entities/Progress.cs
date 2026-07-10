namespace LMS.Domain.Entities;

public class Progress : BaseEntity
{
    public Guid EnrollmentId { get; private set; }
    public Guid LessonId { get; private set; }
    public bool Completed { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public Enrollment Enrollment { get; private set; } = null!;
    public Lesson Lesson { get; private set; } = null!;

    protected Progress() { }

    public Progress(Guid enrollmentId, Guid lessonId)
    {
        EnrollmentId = enrollmentId;
        LessonId = lessonId;
        Completed = false;
    }

    public void Complete()
    {
        Completed = true;
        CompletedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }
}
