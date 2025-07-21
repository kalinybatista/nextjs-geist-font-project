using Microsoft.EntityFrameworkCore;
using NotasFiscaisSystem.Models;

namespace NotasFiscaisSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<ItemNotaFiscal> ItensNotaFiscal { get; set; }
        public DbSet<Empresa> Empresas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotaFiscal>()
                .HasMany(n => n.Itens)
                .WithOne(i => i.NotaFiscal)
                .HasForeignKey(i => i.NotaFiscalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NotaFiscal>()
                .HasIndex(n => n.ChaveAcesso)
                .IsUnique();

            modelBuilder.Entity<Empresa>()
                .HasIndex(e => e.CNPJ)
                .IsUnique();
        }
    }
}
