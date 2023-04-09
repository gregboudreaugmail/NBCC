Declare @LastID INT
Declare @DefaultID INT

INSERT Secure.Users (UserName, [Password], [Hash])
VALUES (@UserName, @Password, @Hash)

SET @LastID = SCOPE_IDENTITY()

SELECT @DefaultID = RoleID FROM Secure.Roles WHERE IsDefault = 1

INSERT INTO [Secure].[UserRoles]
           ([UserID]
           ,[RoleID])
     VALUES
           (@LastID, @DefaultID)

