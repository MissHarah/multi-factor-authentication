namespace SignUpAuthentication.Dto
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string Token {  get; set; }
        public string RefreshToken { get; set; }
    }
}
