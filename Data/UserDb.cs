using System.ComponentModel.DataAnnotations;

namespace SignUpAuthentication.Data
{
    public class UserDb
    {
            [Key]
            public int Id { get; set; }
            [Required]
            public string Username { get; set; }
            public byte[] PasswordHash { get; set; }
            public byte[] PasswordKey { get; set; }
            [Required]
            public string PhoneNumber { get; set; }


        }
    }

