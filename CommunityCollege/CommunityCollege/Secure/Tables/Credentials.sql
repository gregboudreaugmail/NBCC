CREATE TABLE [Secure].[Credentials] (
    [CredentialsID] INT              IDENTITY (1, 1) NOT NULL,
    [UserID]        INT              NOT NULL,
    [Password]      VARBINARY (1024) NOT NULL,
    [Hash]          VARBINARY (1024) NOT NULL,
    CONSTRAINT [FK_Credentials_Users] FOREIGN KEY ([UserID]) REFERENCES [Secure].[Users] ([UserID])
);

