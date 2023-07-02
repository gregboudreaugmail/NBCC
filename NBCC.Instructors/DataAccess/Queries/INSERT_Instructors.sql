
INSERT INTO [Staff].[Instructors]
           ([FirstName]
           ,[LastName]
           ,[Email])
     VALUES
           (@FirstName,
           @LastName,
           @Email)
           
SELECT @@IDENTITY