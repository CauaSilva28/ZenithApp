using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class ConviteTreinador
    {
        [Key]
        public int Id { get; set; }

        public int IdTreinador { get; set; }

        [ForeignKey("IdTreinador")]          // ← diz explicitamente qual coluna é a FK
        public Treinador Treinador { get; set; } = null!;

        public int IdAtleta { get; set; }

        [ForeignKey("IdAtleta")]             // ← mesmo aqui
        public Atleta Atleta { get; set; } = null!;

        public string Status { get; set; } = "Pendente";

        public DateTime DataEnvio { get; set; } = DateTime.Now;
    }
}