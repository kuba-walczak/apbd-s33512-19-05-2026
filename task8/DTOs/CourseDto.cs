namespace task8.DTOs;

public class CourseDto
{
    public int CourseId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Credits { get; set; }
    public int AssignmentCount { get; set; }
}
