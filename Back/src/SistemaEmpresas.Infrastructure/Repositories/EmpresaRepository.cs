using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Domain.Entities;
using SistemaEmpresas.Infrastructure.Data;

namespace SistemaEmpresas.Infrastructure.Repositories;

public class EmpresaRepository : GeralRepository<Empresa>
{
    public EmpresaRepository(ApplicationDbContext context) : base(context)
    {
    }

    //Somente para leitura
    public async Task<Empresa?> GetByIdAsync(Guid id)
    {
        return await Context.Empresas
            .AsNoTracking()
            .Include(e => e.Endereco)
            .Include(e => e.Contato)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    //Para atualização
    public async Task<Empresa?> GetByIdForUpdateAsync(Guid id)
    {
        return await Context.Empresas
            .Include(e => e.Endereco)
            .Include(e => e.Contato)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Empresa?> GetByCnpjAsync(string cnpj)
    {
        return await Context.Empresas
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Cnpj == cnpj);
    }

    public IQueryable<Empresa> Query()
    {
        return Context.Empresas.AsQueryable();
    }
}