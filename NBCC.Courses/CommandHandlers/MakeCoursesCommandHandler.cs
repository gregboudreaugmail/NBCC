using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;

namespace NBCC.Courses.CommandHandlers;

public sealed class MakeCoursesCommandHandler : ICommandHandler<MakeCoursesCommand, int>
{
    ICourseRepository CourseRepository { get; }
    public MakeCoursesCommandHandler(ICourseRepository courseRepository) => CourseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));

    public async Task<int> Handle(MakeCoursesCommand command) => await CourseRepository.Make(command.CourseName);
}