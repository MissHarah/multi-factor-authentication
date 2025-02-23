using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SignUpAuthentication.Model
{
    public class User
    {
        [Required]
        public string Username {  get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordKey { get; set; }
       // [Required]
        public string? PhoneNumber { get; set; }

        
    }
}
