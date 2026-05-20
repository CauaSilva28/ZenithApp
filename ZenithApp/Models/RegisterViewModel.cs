using System.ComponentModel.DataAnnotations;

namespace ZenithApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Informe o nome")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o sobrenome")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o e-mail")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
        public string Password { get; set; } = string.Empty;

        public bool AcceptTerms { get; set; } = false;

        [Required(ErrorMessage = "Selecione o tipo de usuário")]
        public string TipoUsuario { get; set; } = "Atleta";
    }
}