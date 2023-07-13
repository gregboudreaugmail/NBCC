using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;

namespace NBCC.Courses.CommandHandlers;
/*
 * Note 18
 * CQRS handlers
 * Without digging into the code of how the handlers work, the below example reads
 * pretty easily and is a big part of the pattern established.
 * The class name is more or less the command or query name with the suffix Handler.
 * It will use the ICommandHandler interface, which forces us to have an async function
 * named "Handle" with a return type specified in the inheritance line,  Wen no return
 * values are needed, the return type is simply Task.
 * The inheritance line also accepts the command or query associated with it, making
 * the properties available that were passed in.
 */

/*
 * Note 19
 * Everything all at once
 * There's nothing new in the rest of this class but it does use a lot of the techniques
 * we've seen up until now.
 * Reduced imports, single line namespace, omission of private declarations, properties
 * with only a get, lambda style for single line calls, use of DI interfaces and CQRS
 * functions to maintain clean code.  
 */
public sealed class MakeCoursesCommandHandler : ICommandHandler<MakeCoursesCommand, int>
{
    ICourseRepository CourseRepository { get; }
    public MakeCoursesCommandHandler(ICourseRepository courseRepository) => CourseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));

    public async Task<int> Handle(MakeCoursesCommand command) => await CourseRepository.Make(command.CourseName);
}