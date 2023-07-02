CREATE TABLE [Staff].[Instructors] (
    [InstructorId] INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]    VARCHAR (50)   NOT NULL,
    [LastName]     VARCHAR (50)   NOT NULL,
    [Email]        NVARCHAR (255) NOT NULL,
    [IsArchived]   BIT            CONSTRAINT [DF_Instructors_Archived] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Instructors] PRIMARY KEY CLUSTERED ([InstructorId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UniqueEmail]
    ON [Staff].[Instructors]([Email] ASC) WHERE ([isarchived]=(0));

