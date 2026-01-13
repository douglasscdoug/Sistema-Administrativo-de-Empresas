using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Domain.Entities;

namespace SistemaEmpresas.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contato>(
            contato =>
            {
                contato.HasKey(c => c.Id);

                contato.Property(c => c.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
                
                contato.Property(c => c.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                contato.Property(c => c.Telefone)
                    .IsRequired()
                    .HasMaxLength(15);
            }
        );

        modelBuilder.Entity<Empresa>(
            empresa =>
            {
                empresa.HasKey(e => e.Id);

                empresa.Property(e => e.RazaoSocial)
                    .IsRequired()
                    .HasMaxLength(100);

                empresa.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasMaxLength(14);
                
                empresa
                    .HasOne(e => e.Endereco)
                    .WithOne().HasForeignKey<Endereco>(e => e.EmpresaId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                empresa
                    .HasOne(e => e.Contato)
                    .WithOne()
                    .HasForeignKey<Contato>(c => c.EmpresaId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                empresa.HasIndex(e => e.Cnpj).IsUnique();
            }
        );

        modelBuilder.Entity<Endereco>(
            endereco =>
            {
                endereco.HasKey(e => e.Id);

                endereco.Property(e => e.Logradouro)
                    .IsRequired()
                    .HasMaxLength(100);

                endereco.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(10);

                endereco.Property(e => e.Complemento)
                    .HasMaxLength(50);

                endereco.Property(e => e.Bairro)
                    .IsRequired()
                    .HasMaxLength(50);

                endereco.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(50);

                endereco.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(2);

                endereco.Property(e => e.CEP)
                    .IsRequired()
                    .HasMaxLength(8);
            }
        );

        modelBuilder.Entity<Usuario>(
            usuario =>
            {
                usuario.HasKey(u => u.Id);

                usuario.Property(u => u.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                usuario.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                usuario.Property(u => u.SenhaHash)
                    .IsRequired()
                    .HasMaxLength(256);

                usuario.HasIndex(u => u.Email).IsUnique();
            }
        );
    }
}
