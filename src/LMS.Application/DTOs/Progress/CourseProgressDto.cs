namespace LMS.Application.DTOs.Progress;

public class CourseProgressDto
{
    public Guid EnrollmentId { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public int TotalLessons { get; set; }
    public int CompletedLessons { get; set; }
    public decimal CompletionPercentage { get; set; }
    public bool IsCompleted => CompletionPercentage == 100;
    public IEnumerable<ProgressResponseDto> Progresses { get; set; } = [];
}
