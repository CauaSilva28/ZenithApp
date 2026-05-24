using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class MetaSemana
    {
        [Key]
        public int IdMeta { get; set; }

        [Required]
        public string Descricao { get; set; } = string.Empty;

        public bool Concluida { get; set; } = false;

        public int IdAtleta { get; set; }
        [ForeignKey("IdAtleta")]
        public Atleta Atleta { get; set; } = null!;

        public int IdTreinador { get; set; }
        [ForeignKey("IdTreinador")]
        public Treinador Treinador { get; set; } = null!;
    }
}