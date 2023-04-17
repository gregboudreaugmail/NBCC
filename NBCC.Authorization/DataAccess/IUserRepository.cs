namespace NBCC.Authorizaion.DataAccess
{
    public interface IUserRepository
    {
        Task Create(string username, string password, string email);
    }
}
