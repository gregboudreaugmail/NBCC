BEGIN TRY

	BEGIN TRANSACTION  

		Declare @UserID INT
		Declare @DefaultID INT

		INSERT Secure.Users (UserName, Email)
		VALUES (@UserName, @Email)

		SET @UserID = SCOPE_IDENTITY()

		SELECT @DefaultID = RoleID FROM Secure.Roles WHERE IsDefault = 1

		INSERT INTO [Secure].[UserRoles]([UserID],[RoleID])
		VALUES (@UserID, @DefaultID)

		INSERT INTO Secure.Credentials (UserID, [Password], [Hash])
		VALUES (@UserID, @Password, @Hash)

	COMMIT TRANSACTION

END TRY  
BEGIN CATCH 
  IF (@@TRANCOUNT > 0)
   BEGIN
      ROLLBACK TRANSACTION 
   END 

	DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    RAISERROR (@ErrorMessage,@ErrorSeverity,@ErrorState);
  
END CATCH;  
  