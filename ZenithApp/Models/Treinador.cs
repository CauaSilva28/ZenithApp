using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class Treinador
    {
        [Key]
        public int IdTreinador { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        public int? Idade { get; set; }

        [MaxLength(50)]
        public string? Cargo { get; set; }

        // Chave estrangeira
        public int? IdLogin { get; set; }

        [ForeignKey("IdLogin")]
        public Login? Login { get; set; }
    }
}