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
        public DbSet<Alimento> Alimentos { get; set; }
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<RegistroPerformance> RegistrosPerformance { get; set; }
        public DbSet<TreinadorAtleta> TreinadorAtletas { get; set; }
        public DbSet<TreinadorAlimento> TreinadorAlimentos { get; set; }

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

            // TreinadorAtleta — chave composta (N:N)
            modelBuilder.Entity<TreinadorAtleta>()
                .HasKey(ta => new { ta.IdTreinador, ta.IdAtleta });

            modelBuilder.Entity<TreinadorAtleta>()
                .HasOne(ta => ta.Treinador)
                .WithMany(t => t.TreinadorAtletas)
                .HasForeignKey(ta => ta.IdTreinador);

            modelBuilder.Entity<TreinadorAtleta>()
                .HasOne(ta => ta.Atleta)
                .WithMany(a => a.TreinadorAtletas)
                .HasForeignKey(ta => ta.IdAtleta);

            // TreinadorAlimento — chave composta (N:N)
            modelBuilder.Entity<TreinadorAlimento>()
                .HasKey(ta => new { ta.IdTreinador, ta.IdAlimento });

            modelBuilder.Entity<TreinadorAlimento>()
                .HasOne(ta => ta.Treinador)
                .WithMany(t => t.TreinadorAlimentos)
                .HasForeignKey(ta => ta.IdTreinador);

            modelBuilder.Entity<TreinadorAlimento>()
                .HasOne(ta => ta.Alimento)
                .WithMany(a => a.TreinadorAlimentos)
                .HasForeignKey(ta => ta.IdAlimento);

            // RegistroPerformance → Atleta (N:1)
            modelBuilder.Entity<RegistroPerformance>()
                .HasOne(r => r.Atleta)
                .WithMany(a => a.Performances)
                .HasForeignKey(r => r.IdAtleta);

            // Treino → Treinador (N:1)
            modelBuilder.Entity<Treino>()
                .HasOne(t => t.Treinador)
                .WithMany(t => t.Treinos)
                .HasForeignKey(t => t.IdTreinador);
        }
    }
}