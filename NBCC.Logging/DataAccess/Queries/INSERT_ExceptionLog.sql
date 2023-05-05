INSERT INTO [Logging].[Exceptions]
           ([AuthenticationId]
           ,[Exception])
     VALUES
           (@AuthenticationId,
           @Exception)
SELECT SCOPE_IDENTITY()