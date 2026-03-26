using System;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Domain.Entities;
using SistemaEmpresas.Infrastructure.Data;

namespace SistemaEmpresas.Infrastructure.Repositories;

public class UsuarioRepository : GeralRepository<Usuario>
{
    public UsuarioRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Usuario[]> GetAllAsync()
    {
        return await Context.Usuarios
            .AsNoTracking()
            .OrderBy(u => u.Nome)
            .ToArrayAsync();
    }

    public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        return await Context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    
    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await Context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }
}
