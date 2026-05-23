namespace task8.DTOs;

public class SubmissionDto
{
    public int SubmissionId { get; set; }
    public string StudentName { get; set; } = null!;
    public string AssignmentTitle { get; set; } = null!;
    public string RepositoryUrl { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int? Score { get; set; }
    public string? Feedback { get; set; }
}
