SELECT c.[AssignmentId]
      ,c.[CourseId]
      ,c.[InstructorId]
	  ,CO.CourseId
	  ,CO.CourseName
	  ,i.InstructorId
	  ,i.FirstName
	  ,i.LastName
	  ,i.Email
  FROM [College].[Assignments] c
  INNER JOIN College.Courses CO ON C.CourseId = CO.CourseId AND CO.IsArchived=0
  INNER JOIN Staff.Instructors i on i.InstructorId = c.InstructorId and i.IsArchived=0
  WHERE c.InstructorId = @InstructorId
  AND c.IsArchived = 0