namespace NBCC.Authorization.DataAccess
{
    public interface IUserRepository
    {
        Task Create(string username, string password, string email);
    }
}
