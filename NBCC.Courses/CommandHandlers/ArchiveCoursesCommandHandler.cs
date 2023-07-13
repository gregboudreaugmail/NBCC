using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;
using NBCC.Courses.Models;
using NBCC.Requests;

namespace NBCC.Courses.CommandHandlers;

public sealed class ArchiveCoursesCommandHandler : ICommandHandler<ArchiveCoursesCommand>
{
    ICourseRepository CourseRepository { get; }
    IWebRequest WebRequest { get; }
    Alerts Alerts { get; }

    public ArchiveCoursesCommandHandler(ICourseRepository courseRepository, IWebRequest webRequest, Alerts alerts)
    {
        CourseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        WebRequest = webRequest ?? throw new ArgumentNullException(nameof(webRequest));
        Alerts = alerts ?? throw new ArgumentNullException(nameof(alerts));
    }

    /*
     * Note 21
     * Calling another web application
     * The rest of the calls in the course application are simple in the sense that move
     * from the controller, to sql and back and that's it.  I wanted to show an example
     * of how you might call another application.  This is important to avoid having the
     * app for courses needing to apply logic for instructors for example.  The IWebRequest
     * interface will accept a url and property to do just that.
     */

    public async Task Handle(ArchiveCoursesCommand command)
    {
        await CourseRepository.Archive(command.CourseId);
        await WebRequest.PostAsync(Alerts.RequestUrl, command.CourseId);
    }
}
