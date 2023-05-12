CREATE TABLE [College].[Courses] (
    [CourseId]   INT           IDENTITY (1, 1) NOT NULL,
    [CourseName] NVARCHAR (50) NOT NULL,
    [IsArchived] BIT           CONSTRAINT [DF_Courses_IsArchieved] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Courses__3214EC07C37A19F6] PRIMARY KEY CLUSTERED ([CourseId] ASC),
    CONSTRAINT [UQ__Courses__9526E277760C01D7] UNIQUE NONCLUSTERED ([CourseName] ASC)
);



