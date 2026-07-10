namespace LMS.Domain.Entities;

public class Module : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public int Order { get; private set; }
    public Guid CourseId { get; private set; }

    public Course Course { get; private set; } = null!;
    public ICollection<Lesson> Lessons { get; private set; } = [];

    protected Module() { }

    public Module(string title, int order, Guid courseId)
    {
        Title = title;
        Order = order;
        CourseId = courseId;
    }

    public void Update(string title, int order)
    {
        Title = title;
        Order = order;
        MarkAsUpdated();
    }
}
