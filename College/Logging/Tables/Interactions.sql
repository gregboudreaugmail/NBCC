CREATE TABLE [Logging].[Interactions] (
    [InteractionDetailsId] INT            IDENTITY (1, 1) NOT NULL,
    [AuthenticationId]     INT            NOT NULL,
    [AssemblyName]         VARCHAR (100)  NOT NULL,
    [Command]              VARCHAR (50)   NOT NULL,
    [Parameters]           NVARCHAR (MAX) NOT NULL,
    [Timestamp]            DATETIME2 (7)  CONSTRAINT [DF_Interactions_Timestamp] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_InteractionsDetails] PRIMARY KEY CLUSTERED ([InteractionDetailsId] ASC),
    CONSTRAINT [FK_InteractionsDetails_InteractionsDetails] FOREIGN KEY ([AuthenticationId]) REFERENCES [Logging].[Authentication] ([AuthenticationId])
);

