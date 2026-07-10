namespace LMS.Domain.Entities;

public class Lesson : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public int Order { get; private set; }
    public Guid ModuleId { get; private set; }

    public Module Module { get; private set; } = null!;
    public ICollection<Progress> Progresses { get; private set; } = [];

    protected Lesson() { }

    public Lesson(string title, string content, int order, Guid moduleId)
    {
        Title = title;
        Content = content;
        Order = order;
        ModuleId = moduleId;
    }

    public void Update(string title, string content, int order)
    {
        Title = title;
        Content = content;
        Order = order;
        MarkAsUpdated();
    }
}
