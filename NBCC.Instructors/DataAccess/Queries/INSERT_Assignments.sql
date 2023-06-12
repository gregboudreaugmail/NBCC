INSERT INTO [College].[Assignments]
           ([InstructorId],[CourseId])
           
     VALUES (@InstructorId,@CourseId)


SELECT @@IDENTITY