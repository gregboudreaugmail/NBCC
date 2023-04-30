INSERT INTO [Logging].[Authentication]
           ([UserId])
     VALUES
           (@UserId)

SELECT SCOPE_IDENTITY()