using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Acessos.Services;

/// <summary>
/// Serviço para gerar tokens JWT.
/// </summary>
public class TokenService
{
    /// <summary>
    /// Gera um token JWT para o usuário e grupos especificados.
    /// </summary>
    /// <param name="userId">O ID do usuário.</param>
    /// <param name="userLogin">O nome do usuário.</param>
    /// <param name="groupIds">A lista de IDs de grupos aos quais o usuário pertence.</param>
    /// <returns>Um token JWT como uma string.</returns>
    public string GenerateToken(int userId, string userLogin, List<string> groupIds)
    {
        var claims = new List<Claim>
            {
                new Claim("id",userId),
                new Claim("usuario", userId),
                new Claim("loginTimestamp", DateTime.UtcNow.ToString())
            };

        foreach (var groupId in groupIds)
        {
            claims.Add(new Claim("groupId", groupId));
        }

        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123456789012345678901234567890123"));
        var credencial = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credencial);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
