using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class TreinadorAtleta
    {
        public int IdTreinador { get; set; }
        public int IdAtleta { get; set; }

        [ForeignKey("IdTreinador")]
        public Treinador Treinador { get; set; } = null!;

        [ForeignKey("IdAtleta")]
        public Atleta Atleta { get; set; } = null!;
    }
}