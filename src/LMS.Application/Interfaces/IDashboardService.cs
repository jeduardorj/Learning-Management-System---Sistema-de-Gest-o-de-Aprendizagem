using LMS.Application.DTOs.Dashboard;

namespace LMS.Application.Interfaces;

public interface IDashboardService
{
    Task<AdminDashboardDto> GetAdminDashboardAsync();
    Task<StudentDashboardDto> GetStudentDashboardAsync();
}
