using LMS.Application.DTOs.Lessons;

namespace LMS.Application.DTOs.Modules;

public class ModuleWithLessonsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public Guid CourseId { get; set; }
    public IEnumerable<LessonResponseDto> Lessons { get; set; } = [];
}
