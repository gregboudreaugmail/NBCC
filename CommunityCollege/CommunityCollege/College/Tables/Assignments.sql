CREATE TABLE [College].[Assignments] (
    [AssignmentId] INT IDENTITY (1, 1) NOT NULL,
    [CourseId]     INT NOT NULL,
    [InstructorId] INT NOT NULL,
    [IsArchived]   BIT CONSTRAINT [DF_Assignments_IsArchived] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Assignments] PRIMARY KEY CLUSTERED ([AssignmentId] ASC),
    CONSTRAINT [FK_Assignments_Courses] FOREIGN KEY ([CourseId]) REFERENCES [College].[Courses] ([CourseId]),
    CONSTRAINT [FK_Assignments_Instructors] FOREIGN KEY ([InstructorId]) REFERENCES [Staff].[Instructors] ([InstructorId])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UniqueAssignments]
    ON [College].[Assignments]([CourseId] ASC, [InstructorId] ASC) WHERE ([IsArchived]=(0));

