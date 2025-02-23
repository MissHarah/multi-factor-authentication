using Microsoft.EntityFrameworkCore;
using SignUpAuthentication.Model;
using SignUpAuthentication.UserRepositories;

namespace SignUpAuthentication.Data
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

         DbSet<UserDb> Users { get; }
      //  AuthenticationDbContext authenticationDb { get; }



        Task<bool> SaveAsync();

    }
}