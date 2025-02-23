using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SignUpAuthentication.Controllers;
using SignUpAuthentication.Data;
using SignUpAuthentication.Model;
using System.Security.Cryptography;

namespace SignUpAuthentication.UserRepositories
{
    public class UserRepository : IUserRepository
    {
       // private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AuthenticationDbContext _authenticationDbContext;
        public UserRepository( AuthenticationDbContext authenticationDbContext, IMapper mapper)
        {
           
            _authenticationDbContext = authenticationDbContext ?? throw new ArgumentException(nameof(authenticationDbContext));
            _mapper = mapper;
        }
        //  public static List<User> _users = new List<User>();

        public async Task<User> Authenticate(string username, string password)
        {
            var userDb = await _authenticationDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            //var user = _unitOfWork.Users.FirstOrDefault(u => u.Username == username);

            if (userDb != null)
            {
                Console.WriteLine($"Found User: {userDb.Username}, PasswordKey: {Convert.ToBase64String(userDb.PasswordKey)}, PasswordHash: {Convert.ToBase64String(userDb.PasswordHash)}");
            }
            else
            {
                Console.WriteLine("User not found");
            }
            if (userDb == null || userDb.PasswordKey == null)
            {
                Console.WriteLine("PasswordKey is null");
                return null;
            }
            var user = _mapper.Map<User>(userDb);

            if (!MatchPasswordHash(password, userDb.PasswordHash, userDb.PasswordKey))
            {
                Console.WriteLine("password mismatch");
                return null;
            }
            return user;

        }
        private bool MatchPasswordHash(string password, byte[] PasswordHash, byte[] PasswordKey)
        {
            using (var hmac = new HMACSHA512(PasswordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < PasswordHash.Length; i++)
                {
                    if (passwordHash[i] != PasswordHash[i])
                        return false;
                }
                return true;
            }

        }
        public async Task Register(string username, string password, string phoneNumber)
        {
            if (await _authenticationDbContext.Users.AnyAsync(u => u.Username == username))
            {
                throw new Exception("Username is already Taken");
            }
            byte[] passwordHash, passwordKey;
            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            var userDb = new UserDb
            {
            Username = username,
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
            PhoneNumber = phoneNumber 
            
        };

            await _authenticationDbContext.Users.AddAsync(userDb);
            await _authenticationDbContext.SaveChangesAsync();

        }
        public async Task<User> GetUser(string username)
        {
            var userDb = await _authenticationDbContext.Users.FirstOrDefaultAsync(s => s.Username == username);
                return userDb == null ?  null : _mapper.Map<User>(userDb);
        }

        public bool UserAlreadyExists(string username)
        {
            return _authenticationDbContext.Users.Any(x => x.Username == username);
        }

       
    }
}

