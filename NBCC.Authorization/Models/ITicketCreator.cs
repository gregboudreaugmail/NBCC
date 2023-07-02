namespace NBCC.Authorization.Models;

public interface ITicketCreator
{
    Task<AuthenticationTicket> GetTicket(string userName);
}