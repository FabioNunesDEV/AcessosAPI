using Microsoft.OpenApi.Models;
using System.Reflection;
using System;
using Acessos.Data;
using Microsoft.EntityFrameworkCore;
using Acessos.Services;
using Acessos.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Acessos.Authorization;
using Microsoft.AspNetCore.Authorization;


/*
 * retirado comentarios
 */

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

// Configurar autenticação JWT
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Configurar autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CriarPolicy", policy => policy.Requirements.Add(new CriarRequirement()));
    options.AddPolicy("LerPolicy", policy => policy.Requirements.Add(new LerRequirement()));
    options.AddPolicy("AlterarPolicy", policy => policy.Requirements.Add(new AlterarRequirement()));
    options.AddPolicy("DeletarPolicy", policy => policy.Requirements.Add(new DeletarRequirement()));
});

// Registrar o manipulador de autorização
builder.Services.AddSingleton<IAuthorizationHandler, CriarHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, LerHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AlterarHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, DeletarHandler>();

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
