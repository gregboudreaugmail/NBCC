CREATE TABLE [Secure].[UserRoles] (
    [UserRoleID] INT IDENTITY (1, 1) NOT NULL,
    [UserID]     INT NOT NULL,
    [RoleID]     INT NOT NULL,
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleID]) REFERENCES [Secure].[Roles] ([RoleID]),
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserID]) REFERENCES [Secure].[Users] ([UserID])
);

