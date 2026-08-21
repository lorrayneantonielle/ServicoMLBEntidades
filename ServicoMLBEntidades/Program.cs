using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServicoMLBEntidades.Application.Auth;
using ServicoMLBEntidades.Application.Auth.Commands;
using ServicoMLBEntidades.Application.Documentos;
using ServicoMLBEntidades.Application.Familias;
using ServicoMLBEntidades.Application.Membros;
using ServicoMLBEntidades.Domain.Auth;
using ServicoMLBEntidades.Domain.Repositories;
using ServicoMLBEntidades.Domain.Services;
using ServicoMLBEntidades.Infrastructure.Auth;
using ServicoMLBEntidades.Infrastructure.Identity;
using ServicoMLBEntidades.Infrastructure.Persistence;
using ServicoMLBEntidades.Infrastructure.Repositories;
using ServicoMLBEntidades.Infrastructure.Storage;
using ServicoMLBEntidades.Middlewares;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicyName = "AdminFrontend";

// Persistence (EF Core — apenas ORM, schema gerenciado exclusivamente via Liquibase)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Identity + RBAC
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT Bearer
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Configuração 'Jwt:Secret' não encontrada.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Configuração 'Jwt:Issuer' não encontrada.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtIssuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = JwtClaimTypes.Role,
            NameClaimType = JwtRegisteredClaimNames.Sub,
        };
    });

builder.Services.AddAuthorization();

// CORS — origins explícitas, nunca wildcard (research.md §8)
var productionOrigin = builder.Configuration["Cors:ProductionOrigin"];
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        var origins = new List<string> { "http://localhost:5173" };
        if (!string.IsNullOrWhiteSpace(productionOrigin))
        {
            origins.Add(productionOrigin);
        }

        policy.WithOrigins(origins.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Application / Infrastructure services (Clean Architecture — DI compõe as camadas)
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IDocumentoStorageService, LocalDocumentoStorageService>();
builder.Services.AddScoped<IFamiliaRepository, FamiliaRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FamiliaService>();
builder.Services.AddScoped<FamiliaStatusService>();
builder.Services.AddScoped<MembroService>();
builder.Services.AddScoped<DocumentoService>();

builder.Services.AddValidatorsFromAssemblyContaining<LoginCommand>();

builder.Services.AddControllers();

// Swagger/OpenAPI com suporte a JWT Bearer (quickstart.md §3)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MLBEntidades API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Informe o token JWT: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            },
            Array.Empty<string>()
        },
    });
});

var app = builder.Build();

app.UseMiddleware<ProblemDetailsExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
