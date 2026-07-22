namespace LMS.Application.DTOs.Progress;

public class ProgressResponseDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public DateTime? CompletedAt { get; set; }
}
