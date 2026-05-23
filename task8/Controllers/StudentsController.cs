using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task8.Data;
using task8.DTOs;

namespace task8.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly UniversityTasksDbContext _context;

    public StudentsController(UniversityTasksDbContext context)
    {
        _context = context;
    }

    [HttpGet("{idStudent}/dashboard")]
    public async Task<IActionResult> GetDashboard(int idStudent)
    {
        var dashboard = await _context.Students
            .AsNoTracking()
            .Where(s => s.StudentId == idStudent)
            .Select(s => new StudentDashboardDto
            {
                StudentId = s.StudentId,
                IndexNumber = s.IndexNumber,
                FullName = s.FirstName + " " + s.LastName,
                IsActive = s.IsActive,
                Enrollments = s.Enrollments.Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    CourseCode = e.Course.Code,
                    CourseName = e.Course.Name,
                    EnrolledAt = e.EnrolledAt,
                    Status = e.Status
                }).ToList(),
                Submissions = s.Submissions.Select(sub => new SubmissionDto
                {
                    SubmissionId = sub.SubmissionId,
                    StudentName = s.FirstName + " " + s.LastName,
                    AssignmentTitle = sub.Assignment.Title,
                    RepositoryUrl = sub.RepositoryUrl,
                    Status = sub.Status,
                    Score = sub.Score,
                    Feedback = sub.Feedback
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (dashboard == null)
            return NotFound($"Student {idStudent} not found.");

        return Ok(dashboard);
    }
}
