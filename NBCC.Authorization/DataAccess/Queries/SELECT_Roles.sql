SELECT RoleID, RoleName, IsDefault FROM Secure.Roles
WHERE RoleID = @RoleID OR @RoleID IS NULL