using System.ComponentModel.DataAnnotations;

namespace ZenithApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o e-mail")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}