using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;
using NBCC.Courses.Models;
using NBCC.WebRequest;

namespace NBCC.Courses.CommandHandlers;

public sealed class ArchiveCoursesCommandHandler : ICommandHandler<ArchiveCoursesCommand>
{
    ICourseRepository CourseRepository { get; }
    IPost Post { get; }
    Alerts Alerts { get; }

    public ArchiveCoursesCommandHandler(ICourseRepository courseRepository, IPost post, Alerts alerts)
    {
        CourseRepository = courseRepository;
        Post = post;
        Alerts = alerts;
    }

    public async Task Handle(ArchiveCoursesCommand command)
    {
        await CourseRepository.Archive(command.CourseId);
        await Post.PostAsync(Alerts.RequestUrl, command.CourseId);
    }
}
