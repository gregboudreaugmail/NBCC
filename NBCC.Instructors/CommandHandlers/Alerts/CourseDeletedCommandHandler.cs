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
        Alerts = alerts;
        InstructorRepository = instructorRepository;
    }

    public async Task Handle(CourseDeleted command)
    {
        var instructors = await InstructorRepository.GetByCourse(command.CourseId);
        await Alerts.Send(instructors.Select(_ => _.Email), SqlScript.CourseDeletedMessage, SqlScript.CourseDeletedSubject);
    }
}
