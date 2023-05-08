using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;
using NBCC.CQRS.Commands;

namespace NBCC.Courses.CommandHandlers;

public sealed class CoursesCommandHandler : ICommandHandler<CoursesCommand>
{
    ICourseRepository CourseRepository { get; }
    public CoursesCommandHandler(ICourseRepository courseRepository) => CourseRepository = courseRepository;
    
    public async Task Handle(CoursesCommand command) => await CourseRepository.Create(command.CourseName);
}
