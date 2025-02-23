using Microsoft.EntityFrameworkCore;
using SignUpAuthentication.Controllers;
using SignUpAuthentication.Model;
using SignUpAuthentication.UserRepositories;

namespace SignUpAuthentication.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private  IUserRepository _userRepository;
        private readonly AuthenticationDbContext _dbContext;
        public UnitOfWork(IUserRepository userRepository, AuthenticationDbContext dbContext)
        {
         
            _dbContext = dbContext;
            _userRepository = userRepository;
        }
        public IUserRepository UserRepository =>
             _userRepository;

       /* public AuthenticationDbContext authenticationDb =>
            _dbContext;*/

        public DbSet<UserDb> Users => _dbContext.Users;

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
           // return Task.FromResult(true);
        }
    }
}
