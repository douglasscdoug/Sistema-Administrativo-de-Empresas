using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Domain.Entities;
using SistemaEmpresas.Infrastructure.Data;

namespace SistemaEmpresas.API.Seed;

public static class SeedData
{
    public static async Task SeedAdminAsync(ApplicationDbContext context, ILogger logger)
    {
        const string email = "admin@sistemaempresas.com";

        var adminExists = await context.Usuarios.AnyAsync(u => u.Email == email);
        if(adminExists)
        {
            logger.LogInformation("Usuário administrador padrão já existe.");
            return;
        } 
            

        var usuario = new Usuario
        {
            Nome = "Administrador",
            Email = "admin@sistemaempresas.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Ativo = true,
            DataCriacao = DateTime.Now,
            Role = Domain.Enums.UsuarioRole.Administrador
        };

        context.Usuarios.Add(usuario);

        await context.SaveChangesAsync();

        logger.LogInformation("Usuário administrador padrão criado com sucesso.");
    }
}