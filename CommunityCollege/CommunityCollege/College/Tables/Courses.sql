CREATE TABLE [College].[Courses] (
    [CollegeID]  INT           IDENTITY (1, 1) NOT NULL,
    [CourseName] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([CollegeID] ASC),
    UNIQUE NONCLUSTERED ([CourseName] ASC)
);

