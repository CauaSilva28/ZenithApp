using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class Treino
    {
        [Key]
        public int IdTreino { get; set; }

        [MaxLength(50)]
        public string? Tipo { get; set; }

        public int? Duracao { get; set; }
        public DateOnly? DataTreino { get; set; }
        public int? Carga { get; set; }
        public int? IdSistema { get; set; }

        // Chave estrangeira
        public int? IdTreinador { get; set; }

        [ForeignKey("IdTreinador")]
        public Treinador? Treinador { get; set; }
    }
}