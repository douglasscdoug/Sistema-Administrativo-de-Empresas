using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaEmpresas.Infrastructure.Data;

// Usado apenas em design-time (EF Core migrations)

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlite(
            "Data Source=sistema_empresas.db"
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
