using Microsoft.EntityFrameworkCore;
using task8.Data;
using task8.DTOs;
using task8.Models;

namespace task8.Services;

public class SubmissionService
{
    private readonly UniversityTasksDbContext _context;

    public SubmissionService(UniversityTasksDbContext context)
    {
        _context = context;
    }

    public async Task<(SubmissionDto? dto, int statusCode, string? error)> CreateAsync(CreateSubmissionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RepositoryUrl) || !dto.RepositoryUrl.StartsWith("https://"))
            return (null, 400, "RepositoryUrl cannot be blank and must start with https://.");

        var student = await _context.Students.FindAsync(dto.StudentId);
        if (student == null)
            return (null, 404, $"Student {dto.StudentId} not found.");
        if (!student.IsActive)
            return (null, 400, "Student is not active.");

        var assignment = await _context.Assignments
            .FirstOrDefaultAsync(a => a.AssignmentId == dto.AssignmentId);
        if (assignment == null)
            return (null, 404, $"Assignment {dto.AssignmentId} not found.");
        if (!assignment.IsPublished)
            return (null, 400, "Assignment is not published.");

        var enrolled = await _context.Enrollments
            .AsNoTracking()
            .AnyAsync(e => e.StudentId == dto.StudentId
                           && e.CourseId == assignment.CourseId
                           && (e.Status == "Active" || e.Status == "Completed"));
        if (!enrolled)
            return (null, 400, "Student is not enrolled in the course that owns this assignment.");

        var duplicate = await _context.Submissions
            .AsNoTracking()
            .AnyAsync(s => s.StudentId == dto.StudentId && s.AssignmentId == dto.AssignmentId);
        if (duplicate)
            return (null, 409, "Student has already submitted this assignment.");

        var now = DateTime.UtcNow;
        var submission = new Submission
        {
            AssignmentId = dto.AssignmentId,
            StudentId = dto.StudentId,
            RepositoryUrl = dto.RepositoryUrl,
            SubmittedAt = now,
            Status = assignment.IsOverdue(now) ? "Late" : "Submitted"
        };

        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();

        return (new SubmissionDto
        {
            SubmissionId = submission.SubmissionId,
            StudentName = student.FirstName + " " + student.LastName,
            AssignmentTitle = assignment.Title,
            RepositoryUrl = submission.RepositoryUrl,
            Status = submission.Status,
            Score = submission.Score,
            Feedback = submission.Feedback
        }, 201, null);
    }

    public async Task<(SubmissionDto? dto, int statusCode, string? error)> GradeAsync(int submissionId, GradeSubmissionDto dto)
    {
        var submission = await _context.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

        if (submission == null)
            return (null, 404, $"Submission {submissionId} not found.");

        if (dto.Score < 0)
            return (null, 400, "Score cannot be negative.");

        if (dto.Score > submission.Assignment.MaxPoints)
            return (null, 400, $"Score cannot exceed MaxPoints ({submission.Assignment.MaxPoints}).");

        submission.Score = dto.Score;
        submission.Feedback = dto.Feedback;
        submission.Status = "Graded";

        await _context.SaveChangesAsync();

        return (new SubmissionDto
        {
            SubmissionId = submission.SubmissionId,
            StudentName = submission.Student.FirstName + " " + submission.Student.LastName,
            AssignmentTitle = submission.Assignment.Title,
            RepositoryUrl = submission.RepositoryUrl,
            Status = submission.Status,
            Score = submission.Score,
            Feedback = submission.Feedback
        }, 200, null);
    }

    public async Task<(int statusCode, string? error)> DeleteAsync(int submissionId)
    {
        var submission = await _context.Submissions.FindAsync(submissionId);

        if (submission == null)
            return (404, $"Submission {submissionId} not found.");

        if (submission.Status == "Graded")
            return (400, "Cannot delete a graded submission.");

        _context.Submissions.Remove(submission);
        await _context.SaveChangesAsync();

        return (204, null);
    }

}
