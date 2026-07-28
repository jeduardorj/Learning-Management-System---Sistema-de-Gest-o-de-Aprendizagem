using LMS.Application.DTOs.Dashboard;
using LMS.Application.Interfaces;
using LMS.Domain.Enums;
using LMS.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Persistence.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly LmsDbContext _context;

    public DashboardRepository(LmsDbContext context)
    {
        _context = context;
    }

    public async Task<int> CountStudentsAsync()
        => await _context.Users
            .CountAsync(x => x.Role == UserRole.Student);

    public async Task<int> CountCoursesAsync(bool? isActive = null)
    {
        var query = _context.Courses.AsQueryable();
        if (isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);
        return await query.CountAsync();
    }

    public async Task<int> CountEnrollmentsAsync()
        => await _context.Enrollments.CountAsync();

    public async Task<int> CountCertificatesAsync()
        => await _context.Certificates.CountAsync();

    public async Task<decimal> GetAverageCompletionRateAsync()
    {
        var enrollments = await _context.Enrollments
            .Select(x => x.Id)
            .ToListAsync();

        if (!enrollments.Any()) return 0;

        var rates = new List<decimal>();

        foreach (var enrollmentId in enrollments)
        {
            var completed = await _context.Progresses
                .CountAsync(x => x.EnrollmentId == enrollmentId && x.Completed);

            var enrollment = await _context.Enrollments
                .FirstAsync(x => x.Id == enrollmentId);

            var total = await _context.Lessons
                .CountAsync(x => x.Module.CourseId == enrollment.CourseId);

            if (total > 0)
                rates.Add(Math.Round((decimal)completed / total * 100, 2));
        }

        return rates.Any() ? Math.Round(rates.Average(), 2) : 0;
    }

    public async Task<IEnumerable<PopularCourseDto>> GetMostPopularCoursesAsync(int top = 5)
        => await _context.Courses
            .Where(x => x.IsActive)
            .Select(x => new PopularCourseDto
            {
                Id = x.Id,
                Title = x.Title,
                EnrollmentCount = x.Enrollments.Count()
            })
            .OrderByDescending(x => x.EnrollmentCount)
            .Take(top)
            .ToListAsync();

    public async Task<int> CountStudentEnrollmentsAsync(Guid userId)
        => await _context.Enrollments
            .CountAsync(x => x.UserId == userId);

    public async Task<int> CountStudentCertificatesAsync(Guid userId)
        => await _context.Certificates
            .CountAsync(x => x.UserId == userId);

    public async Task<IEnumerable<StudentCourseProgressDto>> GetStudentCoursesProgressAsync(Guid userId)
    {
        var enrollments = await _context.Enrollments
            .Include(x => x.Course)
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var result = new List<StudentCourseProgressDto>();

        foreach (var enrollment in enrollments)
        {
            var total = await _context.Lessons
                .CountAsync(x => x.Module.CourseId == enrollment.CourseId);

            var completed = await _context.Progresses
                .CountAsync(x => x.EnrollmentId == enrollment.Id && x.Completed);

            var percentage = total == 0 ? 0 :
                Math.Round((decimal)completed / total * 100, 2);

            var hasCertificate = await _context.Certificates
                .AnyAsync(x => x.UserId == userId && x.CourseId == enrollment.CourseId);

            result.Add(new StudentCourseProgressDto
            {
                CourseId = enrollment.CourseId,
                CourseTitle = enrollment.Course.Title,
                CompletionPercentage = percentage,
                HasCertificate = hasCertificate
            });
        }

        return result.OrderByDescending(x => x.CompletionPercentage);
    }
}
