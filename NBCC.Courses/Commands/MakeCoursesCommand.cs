/*
 * Note 16
 * class variants and descriptions.
 * New to C# in recent versions is a class variation called a record
 * It is basically a short hand class with little function.  Or at
 * least it should be treated that way.  Unlike 'classes', a record
 * will publicly expose its properties.
 * For example, this record would be the equivalent to a class with a
 * public variable called CourseName and the constructor housing some code
 * to assign that public variable to a private variable passed in.
 *
 * sealed classes and records
 * Just as we developed our controller with the audience in mind (web developer?),
 * we also want to develop our code the same way.  For another developer
 * or our future selves.  Sealing a class only means it can't be inherited by
 * another class.  That is the intent as of the time of writing the code.
 * Be as descriptive with your intent as possible and offer the least
 * amount of class visibility as possible.
 * 
 */

/*
 * Note 17
 * CQRS usage
 * As stated in the controller walk-through, the only information needed to make a
 * course is the name.  As such, this is the only class exposed to my developer audience
 * which shows that intent.
 */

namespace NBCC.Courses.Commands;

public sealed record MakeCoursesCommand(string CourseName);