namespace EcoNet.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
