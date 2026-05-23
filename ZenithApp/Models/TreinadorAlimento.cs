using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class TreinadorAlimento
    {
        public int IdTreinador { get; set; }
        public int IdAlimento { get; set; }

        [ForeignKey("IdTreinador")]
        public Treinador Treinador { get; set; } = null!;

        [ForeignKey("IdAlimento")]
        public Alimento Alimento { get; set; } = null!;
    }
}