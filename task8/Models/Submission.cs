using System;
using System.Collections.Generic;

namespace task8.Models;

public partial class Submission
{
    public int SubmissionId { get; set; }

    public int AssignmentId { get; set; }

    public int StudentId { get; set; }

    public string RepositoryUrl { get; set; } = null!;

    public DateTime SubmittedAt { get; set; }

    public int? Score { get; set; }

    public string? Feedback { get; set; }

    public string Status { get; set; } = null!;

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
