using NBCC.Logging.Models;

namespace NBCC.Logging.DataAccess;

public interface IInteractionLog
{
    Task Log(Interaction? interaction);
}
