using System.ComponentModel.DataAnnotations;

namespace ZenithApp.Models
{
    public class Login
    {
        [Key]
        public int IdLogin { get; set; }

        [Required]
        [MaxLength(50)]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Senha { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? NivelAcesso { get; set; }

        // Navegação
        public Atleta? Atleta { get; set; }
        public Treinador? Treinador { get; set; }
    }
}