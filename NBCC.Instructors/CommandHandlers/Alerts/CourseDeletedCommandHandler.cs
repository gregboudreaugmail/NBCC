using NBCC.Alerts;
using NBCC.Instructors.Commands.Alerts;
using NBCC.Instructors.DataAccess;
using NBCC.Instructors.Properties;

namespace NBCC.Instructors.CommandHandlers.Alerts;

public class CourseDeletedCommandHandler : ICommandHandler<CourseDeleted>
{
    IAlerts Alerts { get; }
    IInstructorRepository InstructorRepository { get; }

    public CourseDeletedCommandHandler(IAlerts alerts, IInstructorRepository instructorRepository)
    {
        Alerts = alerts ?? throw new ArgumentNullException(nameof(alerts));
        InstructorRepository = instructorRepository ?? throw new ArgumentNullException(nameof(instructorRepository));
    }

    /*
     * Note 23
     * Using 3rd parties
     * This example is kind of forced but I wanted to give an example of using a 3rd party and how to
     * keep it separate from your application.  When a course is archived in the course app, it does
     * an HTTP post to the instructors app that will email them with an alert.  Emailing being the
     * "3rd party" (even though it's part of the framework, just pretend).  Like other extracted calls,
     * we'll use an interface but this one will be outside our app in the NBCC.Alerts project.
     */
    public async Task Handle(CourseDeleted command)
    {
        var instructors = await InstructorRepository.GetByCourse(command.CourseId);
        await Alerts.Send(instructors.Select(_ => _.Email), SqlScript.CourseDeletedMessage, SqlScript.CourseDeletedSubject);
    }
}
