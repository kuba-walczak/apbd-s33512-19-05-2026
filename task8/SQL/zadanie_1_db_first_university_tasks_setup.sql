USE master;
GO

IF DB_ID(N'ApbdLecture9DbFirstTask') IS NOT NULL
BEGIN
    ALTER DATABASE ApbdLecture9DbFirstTask SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ApbdLecture9DbFirstTask;
END
GO

CREATE DATABASE ApbdLecture9DbFirstTask;
GO

USE ApbdLecture9DbFirstTask;
GO

CREATE TABLE dbo.Students
(
    StudentId INT IDENTITY(1,1) CONSTRAINT PK_Students PRIMARY KEY,
    IndexNumber NVARCHAR(20) NOT NULL,
    FirstName NVARCHAR(80) NOT NULL,
    LastName NVARCHAR(80) NOT NULL,
    Email NVARCHAR(160) NOT NULL,
    EnrollmentDate DATE NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Students_IsActive DEFAULT 1,
    CONSTRAINT UQ_Students_IndexNumber UNIQUE (IndexNumber),
    CONSTRAINT UQ_Students_Email UNIQUE (Email)
);

CREATE TABLE dbo.Courses
(
    CourseId INT IDENTITY(1,1) CONSTRAINT PK_Courses PRIMARY KEY,
    Code NVARCHAR(20) NOT NULL,
    Name NVARCHAR(160) NOT NULL,
    Credits INT NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Courses_IsActive DEFAULT 1,
    CONSTRAINT UQ_Courses_Code UNIQUE (Code),
    CONSTRAINT CK_Courses_Credits CHECK (Credits BETWEEN 1 AND 10)
);

CREATE TABLE dbo.Enrollments
(
    EnrollmentId INT IDENTITY(1,1) CONSTRAINT PK_Enrollments PRIMARY KEY,
    StudentId INT NOT NULL,
    CourseId INT NOT NULL,
    EnrolledAt DATE NOT NULL,
    Status NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_Enrollments_Students FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId),
    CONSTRAINT FK_Enrollments_Courses FOREIGN KEY (CourseId) REFERENCES dbo.Courses(CourseId),
    CONSTRAINT UQ_Enrollments_Student_Course UNIQUE (StudentId, CourseId),
    CONSTRAINT CK_Enrollments_Status CHECK (Status IN (N'Active', N'Completed', N'Dropped'))
);

CREATE TABLE dbo.Assignments
(
    AssignmentId INT IDENTITY(1,1) CONSTRAINT PK_Assignments PRIMARY KEY,
    CourseId INT NOT NULL,
    Title NVARCHAR(160) NOT NULL,
    Description NVARCHAR(1000) NULL,
    DueDate DATETIME2 NOT NULL,
    MaxPoints INT NOT NULL,
    IsPublished BIT NOT NULL CONSTRAINT DF_Assignments_IsPublished DEFAULT 0,
    CONSTRAINT FK_Assignments_Courses FOREIGN KEY (CourseId) REFERENCES dbo.Courses(CourseId),
    CONSTRAINT CK_Assignments_MaxPoints CHECK (MaxPoints > 0)
);

CREATE TABLE dbo.Submissions
(
    SubmissionId INT IDENTITY(1,1) CONSTRAINT PK_Submissions PRIMARY KEY,
    AssignmentId INT NOT NULL,
    StudentId INT NOT NULL,
    RepositoryUrl NVARCHAR(300) NOT NULL,
    SubmittedAt DATETIME2 NOT NULL,
    Score INT NULL,
    Feedback NVARCHAR(1000) NULL,
    Status NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_Submissions_Assignments FOREIGN KEY (AssignmentId) REFERENCES dbo.Assignments(AssignmentId),
    CONSTRAINT FK_Submissions_Students FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId),
    CONSTRAINT UQ_Submissions_Assignment_Student UNIQUE (AssignmentId, StudentId),
    CONSTRAINT CK_Submissions_Status CHECK (Status IN (N'Submitted', N'Late', N'Graded')),
    CONSTRAINT CK_Submissions_Score CHECK (Score IS NULL OR Score >= 0)
);
GO

CREATE INDEX IX_Enrollments_CourseId ON dbo.Enrollments(CourseId);
CREATE INDEX IX_Assignments_CourseId ON dbo.Assignments(CourseId);
CREATE INDEX IX_Assignments_DueDate ON dbo.Assignments(DueDate);
CREATE INDEX IX_Submissions_StudentId ON dbo.Submissions(StudentId);
CREATE INDEX IX_Submissions_AssignmentId ON dbo.Submissions(AssignmentId);
CREATE INDEX IX_Submissions_Status ON dbo.Submissions(Status);
GO

INSERT INTO dbo.Students (IndexNumber, FirstName, LastName, Email, EnrollmentDate, IsActive)
VALUES
(N's30001', N'Anna', N'Kowalska', N'anna.kowalska@students.example.edu', '2023-10-01', 1),
(N's30002', N'Jan', N'Nowak', N'jan.nowak@students.example.edu', '2023-10-01', 1),
(N's30003', N'Maria', N'Zielinska', N'maria.zielinska@students.example.edu', '2023-10-01', 1),
(N's30004', N'Piotr', N'Wisniewski', N'piotr.wisniewski@students.example.edu', '2023-10-01', 1),
(N's30005', N'Katarzyna', N'Lewandowska', N'katarzyna.lewandowska@students.example.edu', '2024-02-15', 1),
(N's30006', N'Tomasz', N'Kaminski', N'tomasz.kaminski@students.example.edu', '2024-02-15', 1),
(N's30007', N'Ewa', N'Wojcik', N'ewa.wojcik@students.example.edu', '2024-02-15', 1),
(N's30008', N'Adam', N'Kaczmarek', N'adam.kaczmarek@students.example.edu', '2024-02-15', 0);

INSERT INTO dbo.Courses (Code, Name, Credits, IsActive)
VALUES
(N'APBD', N'Database Applications', 5, 1),
(N'PGO', N'Object-Oriented Programming', 5, 1),
(N'ABD', N'Advanced Databases', 4, 1),
(N'IDH', N'Data Warehousing and Analytics', 4, 1),
(N'LEGACY', N'Legacy Systems Integration', 3, 0);

INSERT INTO dbo.Enrollments (StudentId, CourseId, EnrolledAt, Status)
VALUES
(1, 1, '2024-10-01', N'Active'),
(1, 2, '2024-10-01', N'Active'),
(2, 1, '2024-10-01', N'Active'),
(2, 3, '2024-10-01', N'Active'),
(3, 1, '2024-10-01', N'Active'),
(3, 4, '2024-10-01', N'Active'),
(4, 1, '2024-10-01', N'Active'),
(4, 2, '2024-10-01', N'Completed'),
(5, 1, '2025-02-20', N'Active'),
(5, 3, '2025-02-20', N'Active'),
(6, 1, '2025-02-20', N'Active'),
(6, 4, '2025-02-20', N'Active'),
(7, 2, '2025-02-20', N'Active'),
(8, 1, '2025-02-20', N'Dropped');

INSERT INTO dbo.Assignments (CourseId, Title, Description, DueDate, MaxPoints, IsPublished)
VALUES
(1, N'EF Core Database First', N'Create a SQL Server database, scaffold EF Core classes, and implement selected endpoints.', '2026-06-15T23:59:00', 20, 1),
(1, N'CRUD REST API', N'Implement CRUD endpoints using EF Core and DTOs.', '2026-06-22T23:59:00', 30, 1),
(1, N'Performance Review', N'Find N+1 query issues and fix them with Include or projections.', '2026-06-29T23:59:00', 25, 1),
(2, N'Interfaces and Abstract Classes', N'Prepare a Java application using interfaces and abstract classes.', '2026-06-18T23:59:00', 20, 1),
(3, N'Indexes and Query Plans', N'Analyze query plans and propose useful indexes.', '2026-06-25T23:59:00', 25, 1),
(4, N'Star Schema Design', N'Design a star schema from OLTP data sources.', '2026-07-02T23:59:00', 30, 1),
(1, N'Final API Project', N'Build a small API using EF Core and DTOs.', '2026-07-10T23:59:00', 40, 0);

INSERT INTO dbo.Submissions (AssignmentId, StudentId, RepositoryUrl, SubmittedAt, Score, Feedback, Status)
VALUES
(1, 1, N'https://github.com/example/s30001-db-first', '2026-06-10T18:20:00', 18, N'Good scaffold command and clear model structure.', N'Graded'),
(1, 2, N'https://github.com/example/s30002-db-first', '2026-06-11T12:10:00', 15, N'Missing partial class explanation.', N'Graded'),
(1, 3, N'https://github.com/example/s30003-db-first', '2026-06-09T09:45:00', 19, N'Very clean solution.', N'Graded'),
(1, 4, N'https://github.com/example/s30004-db-first', '2026-06-16T10:30:00', 12, N'Late, but mostly correct.', N'Graded'),
(1, 5, N'https://github.com/example/s30005-db-first', '2026-06-12T21:00:00', NULL, NULL, N'Submitted'),
(2, 1, N'https://github.com/example/s30001-crud-api', '2026-06-20T15:30:00', 27, N'Good use of DTOs.', N'Graded'),
(2, 2, N'https://github.com/example/s30002-crud-api', '2026-06-21T11:00:00', 21, N'Repository layer needs cleanup.', N'Graded'),
(2, 3, N'https://github.com/example/s30003-crud-api', '2026-06-19T19:15:00', 29, N'Excellent service layer.', N'Graded'),
(2, 6, N'https://github.com/example/s30006-crud-api', '2026-06-23T08:40:00', NULL, NULL, N'Late'),
(5, 2, N'https://github.com/example/s30002-indexes', '2026-06-24T17:00:00', 22, N'Correct indexes.', N'Graded'),
(5, 5, N'https://github.com/example/s30005-indexes', '2026-06-25T20:00:00', NULL, NULL, N'Submitted'),
(6, 3, N'https://github.com/example/s30003-star-schema', '2026-06-28T14:00:00', 26, N'Good dimensional model.', N'Graded'),
(6, 6, N'https://github.com/example/s30006-star-schema', '2026-06-29T16:10:00', NULL, NULL, N'Submitted');
GO

SELECT
    s.StudentId,
    s.IndexNumber,
    s.FirstName,
    s.LastName,
    COUNT(sub.SubmissionId) AS SubmissionCount
FROM dbo.Students AS s
LEFT JOIN dbo.Submissions AS sub ON sub.StudentId = s.StudentId
GROUP BY s.StudentId, s.IndexNumber, s.FirstName, s.LastName
ORDER BY s.StudentId;
GO
