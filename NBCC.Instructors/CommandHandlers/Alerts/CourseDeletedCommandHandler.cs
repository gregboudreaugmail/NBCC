using NBCC.Alerts;
using NBCC.Instructors.Commands.Alerts;

namespace NBCC.Instructors.CommandHandlers.Alerts;

public class CourseDeletedCommandHandler : ICommandHandler<CourseDeleted>
{
    public IAlerts Alerts { get; }

    public CourseDeletedCommandHandler(IAlerts alerts) => Alerts = alerts;

    public async Task Handle(CourseDeleted command)
    {
        await Alerts.Send(command.CourseId);
    }
}
