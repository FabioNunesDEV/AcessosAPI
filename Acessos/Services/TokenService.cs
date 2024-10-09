using Acessos.Data;
using Acessos.DTO.Auth;
using Acessos.DTO.Grupo;
using Acessos.Models;
using Acessos.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Acessos.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Acessos.Services;

/// <summary>
/// Serviço de token para gerenciar tokens JWT.
/// </summary>
public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Gera um token JWT.
    /// </summary>
    /// <param name="userId">ID do usuário.</param>
    /// <param name="userLogin">Login do usuário.</param>
    /// <param name="groupIds">IDs dos grupos do usuário.</param>
    /// <param name="permissao">Permissões do usuário.</param>
    /// <returns>Um token JWT como uma string.</returns>
    public string GenerateToken(string userId, string userLogin, List<string> groupIds, GrupoPermissaoDTO permissao)
    {
        var claims = new List<Claim>
        {
            new Claim("id", userId),
            new Claim("login", userLogin)
        };

        claims.AddRange(groupIds.Select(groupId => new Claim("grupos", groupId)));

        if (permissao.PodeCriar) claims.Add(new Claim("permissao", "criar"));
        if (permissao.PodeLer) claims.Add(new Claim("permissao", "ler"));
        if (permissao.PodeAlterar) claims.Add(new Claim("permissao", "alterar"));
        if (permissao.PodeDeletar) claims.Add(new Claim("permissao", "deletar"));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:_jwtSettings.Issuer,
            audience:_jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}
