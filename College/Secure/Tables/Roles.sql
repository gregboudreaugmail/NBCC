CREATE TABLE [Secure].[Roles] (
    [RoleID]    INT          IDENTITY (1, 1) NOT NULL,
    [RoleName]  VARCHAR (50) NOT NULL,
    [IsDefault] BIT          CONSTRAINT [DF_Roles_IsDefault] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleID] ASC),
    UNIQUE NONCLUSTERED ([IsDefault] ASC),
    CONSTRAINT [UQ__Roles__8A2B616049661658] UNIQUE NONCLUSTERED ([RoleName] ASC)
);

