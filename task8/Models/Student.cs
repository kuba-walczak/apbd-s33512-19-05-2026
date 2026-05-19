using System;
using System.Collections.Generic;

namespace task8.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string IndexNumber { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly EnrollmentDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
