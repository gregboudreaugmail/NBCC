using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;
using NBCC.CQRS.Commands;
using NBCC.WebRequest;

namespace NBCC.Courses.CommandHandlers;

public sealed class MakeCoursesCommandHandler : ICommandHandler<MakeCoursesCommand>
{
    ICourseRepository CourseRepository { get; }
    IPost Post { get; }
    public MakeCoursesCommandHandler(ICourseRepository courseRepository, IPost iPost)
    {
        CourseRepository = courseRepository;
        Post = iPost;
    }

    public async Task Handle(MakeCoursesCommand command)
    {
        await CourseRepository.Make(command.CourseName);
        await Post.PostAsync(new CourseAssignment { CourseId = 3 });
    }
}