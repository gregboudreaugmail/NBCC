CREATE TABLE [Logging].[Authentication] (
    [AuthenticationId] INT           IDENTITY (1, 1) NOT NULL,
    [UserId]           INT           NOT NULL,
    [Timestamp]        DATETIME2 (7) CONSTRAINT [DF_Authentication_Timestamp] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Interactions] PRIMARY KEY CLUSTERED ([AuthenticationId] ASC),
    CONSTRAINT [FK_Interactions_Users] FOREIGN KEY ([UserId]) REFERENCES [Secure].[Users] ([UserID])
);

