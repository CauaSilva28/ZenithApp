using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class Treino
    {
        [Key]
        public int IdTreino { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string? Observacoes { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Quem criou
        public int IdTreinador { get; set; }
        [ForeignKey("IdTreinador")]
        public Treinador Treinador { get; set; } = null!;

        // Para qual atleta
        public int IdAtleta { get; set; }
        [ForeignKey("IdAtleta")]
        public Atleta Atleta { get; set; } = null!;

        // Exercícios do treino
        public List<Exercicio> Exercicios { get; set; } = new();

        public bool Concluido { get; set; } = false;

        public DateTime? DataConclusao { get; set; }
    }
}