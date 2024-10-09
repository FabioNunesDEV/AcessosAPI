using Microsoft.OpenApi.Models;
using System.Reflection;
using System;
using Acessos.Data;
using Microsoft.EntityFrameworkCore;
using Acessos.Services;
using Acessos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Add Automapper a aplicação
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add classes de serviços
builder.Services.AddScoped<CircularesService>();
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<GruposService>();
builder.Services.AddScoped<AuthService>();

// Carregar configurações do JWT do appsettings.json
var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

// Add TokenService 
builder.Services.AddScoped<ITokenService, TokenService>();

// Definido conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("AcessoAPIConnection");
builder.Services.AddDbContext<AcessoApiContext>(opts => opts.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AcessosAPI", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
