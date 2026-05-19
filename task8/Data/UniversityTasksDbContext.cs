using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using task8.Models;

namespace task8.Data;

public partial class UniversityTasksDbContext : DbContext
{
    public UniversityTasksDbContext(DbContextOptions<UniversityTasksDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_Assignments_CourseId");

            entity.HasIndex(e => e.DueDate, "IX_Assignments_DueDate");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(160);

            entity.HasOne(d => d.Course).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignments_Courses");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasIndex(e => e.Code, "UQ_Courses_Code").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Courses_IsActive");
            entity.Property(e => e.Name).HasMaxLength(160);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_Enrollments_CourseId");

            entity.HasIndex(e => new { e.StudentId, e.CourseId }, "UQ_Enrollments_Student_Course").IsUnique();

            entity.Property(e => e.Status).HasMaxLength(30);

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_Courses");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ_Students_Email").IsUnique();

            entity.HasIndex(e => e.IndexNumber, "UQ_Students_IndexNumber").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(160);
            entity.Property(e => e.FirstName).HasMaxLength(80);
            entity.Property(e => e.IndexNumber).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Students_IsActive");
            entity.Property(e => e.LastName).HasMaxLength(80);
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasIndex(e => e.AssignmentId, "IX_Submissions_AssignmentId");

            entity.HasIndex(e => e.Status, "IX_Submissions_Status");

            entity.HasIndex(e => e.StudentId, "IX_Submissions_StudentId");

            entity.HasIndex(e => new { e.AssignmentId, e.StudentId }, "UQ_Submissions_Assignment_Student").IsUnique();

            entity.Property(e => e.Feedback).HasMaxLength(1000);
            entity.Property(e => e.RepositoryUrl).HasMaxLength(300);
            entity.Property(e => e.Status).HasMaxLength(30);

            entity.HasOne(d => d.Assignment).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Assignments");

            entity.HasOne(d => d.Student).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Students");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
