using System.ComponentModel.DataAnnotations;

namespace SignUpAuthentication.Dto
{
    public class LoginRequest
    {
        [Required]
        public string username {  get; set; }
        [Required]
        public string password { get; set; }

        public string phoneNumber {  get; set; }
    }
}
