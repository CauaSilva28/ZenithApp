using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class Atleta
    {
        [Key]
        public int IdAtleta { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        public int? Idade { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? Altura { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? Largura { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Peso { get; set; }

        // Chave estrangeira
        public int? IdLogin { get; set; }

        [ForeignKey("IdLogin")]
        public Login? Login { get; set; }
    }
}