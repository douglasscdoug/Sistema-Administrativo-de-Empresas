using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Application.Helpers;
using SistemaEmpresas.Infrastructure.Repositories;
using SistemaEmpresas.Infrastructure.Data;
using SistemaEmpresas.Application.Interfaces;
using SistemaEmpresas.Application.Services;
using SistemaEmpresas.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(SistemaEmpresasProfile));

builder.Services.AddScoped<EmpresaRepository>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
