using System;
using System.Collections.Generic;

namespace task8.Models;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public int MaxPoints { get; set; }

    public bool IsPublished { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
