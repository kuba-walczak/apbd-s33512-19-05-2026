namespace task8.DTOs;

public class CreateSubmissionDto
{
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    public string RepositoryUrl { get; set; } = null!;
}
