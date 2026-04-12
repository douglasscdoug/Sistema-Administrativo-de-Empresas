using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Application.Helpers;
using SistemaEmpresas.Infrastructure.Repositories;
using SistemaEmpresas.Infrastructure.Data;
using SistemaEmpresas.Application.Interfaces;
using SistemaEmpresas.Application.Services;
using SistemaEmpresas.API.Middlewares;
using FluentValidation.AspNetCore;
using SistemaEmpresas.Application.Validators;
using FluentValidation;
using SistemaEmpresas.Application.Auth.Settings;
using SistemaEmpresas.Application.Auth.Interfaces;
using SistemaEmpresas.Application.Auth.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurações
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()
    ?? throw new Exception("JwtSettings não configurada.");

if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
    throw new Exception("JwtSettings não configurada ou SecretKey vazia.");

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

//2. DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//3.Repositórios
builder.Services.AddScoped<EmpresaRepository>();
builder.Services.AddScoped<UsuarioRepository>();

// 4. Services
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// 5. FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<EmpresaDtoValidator>();

// 6. Autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

//7. Authorization
builder.Services.AddAuthorization();

//8. Controllers e AutoMapper
builder.Services.AddAutoMapper(typeof(SistemaEmpresasProfile));
builder.Services.AddControllers();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SistemaEmpresas API",
        Version = "v1",
        Description = "API para gerenciamento de empresas e usuários."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
