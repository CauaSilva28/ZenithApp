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

        [Column(TypeName = "decimal(5,2)")]
        public decimal? CompCorporal { get; set; }

        [MaxLength(50)]
        public string? Biotipo { get; set; }

        // Chave estrangeira
        public int? IdLogin { get; set; }

        [ForeignKey("IdLogin")]
        public Login? Login { get; set; }

        // Navegação
        public ICollection<RegistroPerformance> Performances { get; set; } = new List<RegistroPerformance>();
        public ICollection<TreinadorAtleta> TreinadorAtletas { get; set; } = new List<TreinadorAtleta>();
    }
}