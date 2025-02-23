using SignUpAuthentication.Model;

namespace SignUpAuthentication.UserRepositories
{
    public interface IUserRepository
    {
        Task Register( string password, string username, string phoneNumber);
        Task<User> Authenticate(string username, string password);
      //  User Authenticate(string username, string password);
        bool UserAlreadyExists(string username);
      //  User GetUser(string username);
        Task<User> GetUser(string username);

    }
}