using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;
using NBCC.CQRS.Commands;

namespace NBCC.Courses.CommandHandlers;

public sealed class MakeCoursesCommandHandler : ICommandHandler<MakeCoursesCommand>
{
    ICourseRepository CourseRepository { get; }
    public MakeCoursesCommandHandler(ICourseRepository courseRepository) => CourseRepository = courseRepository;
    
    public async Task Handle(MakeCoursesCommand command) => await CourseRepository.Make(command.CourseName);
}