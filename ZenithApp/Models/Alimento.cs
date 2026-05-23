using System.ComponentModel.DataAnnotations;

namespace ZenithApp.Models
{
    public class Alimento
    {
        [Key]
        public int IdAlimento { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        public int? Calorias { get; set; }
        public int? Proteinas { get; set; }
        public int? Carboidratos { get; set; }
        public int? IdSistema { get; set; }

        // Navegação
        public ICollection<TreinadorAlimento> TreinadorAlimentos { get; set; } = new List<TreinadorAlimento>();
    }
}