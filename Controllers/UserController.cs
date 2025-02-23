using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using SignUpAuthentication.Data;
using SignUpAuthentication.Dto;
using SignUpAuthentication.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace SignUpAuthentication.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class UserController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _config = config;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {

           var user = await _unitOfWork.UserRepository.Authenticate(loginRequest.username, loginRequest.password);
            ApiError apiError = new ApiError();
            if(user == null)
            {
                apiError.ErrorCode = Unauthorized().StatusCode;
                apiError.ErrorMessage = "invalid username or password";
                apiError.ErrorDetails = "this error appear when provided user name or password does not exist";

                //Console.WriteLine($"User authenticated: {user.Username}");
                return Unauthorized(apiError);
            }
            var loginResponse = new LoginResponse();
            loginResponse.Username = user.Username;
            loginResponse.Token = CreateJWT(user);
            loginResponse.RefreshToken = GenerateRefreshToken();
            
            return Ok(loginResponse);
                
           /* await _unitOfWork.SaveAsync();
            return StatusCode(201);*/

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginRequest loginRequest)
        {
            ApiError apiError = new ApiError();
            if( _unitOfWork.UserRepository.UserAlreadyExists(loginRequest.username))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User already exist, please try different user name";
                return BadRequest(apiError);
            }

            await _unitOfWork.UserRepository.Register(loginRequest.username, loginRequest.password, loginRequest.phoneNumber);
            await _unitOfWork.SaveAsync();
            return Ok("Registration is succesful");
        }
        private string CreateJWT(User user) 
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Appsettings:Token"]));
            var claims = new Claim[]
            
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber ?? "")
            };
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10)

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var reft = RandomNumberGenerator.Create())
            {
                reft.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

    }
}
