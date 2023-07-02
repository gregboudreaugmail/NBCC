CREATE TABLE [Logging].[Exceptions] (
    [ExceptionId]      INT            IDENTITY (1, 1) NOT NULL,
    [AuthenticationId] INT            NULL,
    [Exception]        NVARCHAR (MAX) NOT NULL,
    [Timestamp]        DATETIME2 (7)  CONSTRAINT [DF_Exceptions_Timestamp] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Exceptions] PRIMARY KEY CLUSTERED ([ExceptionId] ASC),
    CONSTRAINT [FK_Exceptions_Authentication] FOREIGN KEY ([AuthenticationId]) REFERENCES [Logging].[Authentication] ([AuthenticationId])
);



