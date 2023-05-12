using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;

namespace NBCC.Courses.CommandHandlers;

public sealed class ArchiveCoursesCommandHandler : ICommandHandler<ArchiveCoursesCommand>
{
    ICourseRepository CourseRepository { get; }
    public ArchiveCoursesCommandHandler(ICourseRepository courseRepository) => CourseRepository = courseRepository;

    public async Task Handle(ArchiveCoursesCommand command) => await CourseRepository.Archive(command.CourseId);
}