namespace task8.DTOs;

public class EnrollmentDto
{
    public int EnrollmentId { get; set; }
    public string CourseCode { get; set; } = null!;
    public string CourseName { get; set; } = null!;
    public DateOnly EnrolledAt { get; set; }
    public string Status { get; set; } = null!;
}
