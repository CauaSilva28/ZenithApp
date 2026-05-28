using Microsoft.EntityFrameworkCore;
using ZenithApp.Models;

namespace ZenithApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Login> Logins { get; set; }
        public DbSet<Atleta> Atletas { get; set; }
        public DbSet<Treinador> Treinadores { get; set; }
        public DbSet<ConviteTreinador> ConvitesTreinador { get; set; }
        public DbSet<TreinadorAtleta> TreinadorAtletas { get; set; }
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<MetaSemana> MetasSemana { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Login — e-mail único
            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasIndex(l => l.Usuario).IsUnique();
            });

            // Treinador → Login (1:1)
            modelBuilder.Entity<Treinador>()
                .HasOne(t => t.Login)
                .WithOne(l => l.Treinador)
                .HasForeignKey<Treinador>(t => t.IdLogin);

            // Atleta → Login (1:1)
            modelBuilder.Entity<Atleta>()
                .HasOne(a => a.Login)
                .WithOne(l => l.Atleta)
                .HasForeignKey<Atleta>(a => a.IdLogin);

            modelBuilder.Entity<TreinadorAtleta>()
            .HasKey(x => new
            {
                x.IdTreinador,
                x.IdAtleta
            });

            modelBuilder.Entity<ConviteTreinador>()
                .HasIndex(x => new { x.IdTreinador, x.IdAtleta })
                .IsUnique();
        }
    }
}