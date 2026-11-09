using Microsoft.EntityFrameworkCore;
using SistemaEventos.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SistemaEventos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<EventoModel> Eventos { get; set; }
        public DbSet<InscricaoModel> Inscricoes { get; set; }
        public DbSet<PresencaModel> Presencas { get; set; }
        public DbSet<CertificadoModel> Certificados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioModel>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UsuarioModel>()
                .HasIndex(u => u.Ra)
                .IsUnique()
                .HasFilter("[Ra] IS NOT NULL");

            modelBuilder.Entity<UsuarioModel>()
                .HasIndex(u => u.Tipo)
                .IsUnique()
                .HasFilter("[Tipo] = 'Diretor'");

            modelBuilder.Entity<InscricaoModel>()
                .HasIndex(i => new { i.UsuarioId, i.EventoId })
                .IsUnique();

            modelBuilder.Entity<PresencaModel>()
                .HasIndex(p => new { p.UsuarioId, p.EventoId })
                .IsUnique();

            modelBuilder.Entity<CertificadoModel>()
                .HasIndex(c => new { c.UsuarioId, c.EventoId })
                .IsUnique();

            modelBuilder.Entity<InscricaoModel>()
                .HasOne(i => i.Usuario)
                .WithMany()
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InscricaoModel>()
                .HasOne(i => i.Evento)
                .WithMany()
                .HasForeignKey(i => i.EventoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PresencaModel>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PresencaModel>()
                .HasOne(p => p.Evento)
                .WithMany()
                .HasForeignKey(p => p.EventoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CertificadoModel>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CertificadoModel>()
                .HasOne(c => c.Evento)
                .WithMany()
                .HasForeignKey(c => c.EventoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventoModel>()
                .HasOne(e => e.Coordenador)
                .WithMany()
                .HasForeignKey(e => e.CoordenadorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}