using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithApp.Models
{
    public class Exercicio
    {
        [Key]
        public int IdExercicio { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public int Series { get; set; }
        public int Repeticoes { get; set; }

        public int IdTreino { get; set; }
        [ForeignKey("IdTreino")]
        public Treino Treino { get; set; } = null!;
    }
}