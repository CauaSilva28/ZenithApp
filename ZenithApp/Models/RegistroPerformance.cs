using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class RegistroPerformance
    {
        [Key]
        public int IdRegistro { get; set; }

        public int? Forca { get; set; }
        public int? Velocidade { get; set; }
        public int? Cardio { get; set; }

        // Chave estrangeira
        public int IdAtleta { get; set; }

        [ForeignKey("IdAtleta")]
        public Atleta Atleta { get; set; } = null!;
    }
}