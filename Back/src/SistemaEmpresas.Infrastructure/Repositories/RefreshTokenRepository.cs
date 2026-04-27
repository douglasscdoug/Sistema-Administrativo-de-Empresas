using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Domain.Entities;
using SistemaEmpresas.Infrastructure.Data;

namespace SistemaEmpresas.Infrastructure.Repositories;

public class RefreshTokenRepository : GeralRepository<RefreshToken>
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context) { }
    
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await Context.RefreshTokens
            .Include(rt => rt.Usuario)
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }
}