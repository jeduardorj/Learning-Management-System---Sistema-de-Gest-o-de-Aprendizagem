namespace LMS.Domain.Entities;

public class Course : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public ICollection<Module> Modules { get; private set; } = [];
    public ICollection<Enrollment> Enrollments { get; private set; } = [];
    public ICollection<Certificate> Certificates { get; private set; } = [];

    protected Course() { }

    public Course(string title, string description)
    {
        Title = title;
        Description = description;
        IsActive = true;
    }

    public void Update(string title, string description)
    {
        Title = title;
        Description = description;
        MarkAsUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        MarkAsUpdated();
    }
}
