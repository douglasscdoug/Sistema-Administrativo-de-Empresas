using System;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Infrastructure.Data;

namespace SistemaEmpresas.Infrastructure.Repositories;

/// <summary>
/// Repositório genérico base para operações CRUD
/// </summary>

public abstract class GeralRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> DbSet;
    public GeralRepository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity)
        => await DbSet.AddAsync(entity);

    public void Update(T entity)
        => DbSet.Update(entity);

    public void Remove(T entity)
        => DbSet.Remove(entity);

    public async Task<bool> SaveChangesAsync()
        => (await Context.SaveChangesAsync()) > 0;
}
