using Acessos.Data;
using Acessos.DTO.Auth;
using Acessos.DTO.Grupo;
using Acessos.Models;
using Acessos.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Acessos.Services;

/// <summary>
/// Serviço de autenticação para gerenciar tokens JWT.
/// </summary>
public class AuthService
{
    private readonly AcessoApiContext _context;


    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="AuthService"/>.
    /// </summary>
    /// <param name="context">O contexto do banco de dados.</param>
    /// <param name="tokenService">O serviço de token.</param>
    public AuthService(AcessoApiContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém um token JWT para o usuário especificado.
    /// </summary>
    /// <param name="dto">Objeto AuthReadDTO.</param>
    /// <returns>Um token JWT como uma string ou uma mensagem de erro.</returns>
    public string ObterToken(AuthReadDTO dto)
    {
        // Obter informações do usuário (exemplo simplificado)
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Login == dto.Login);
        if (usuario == null)
        {
            return "Usuário não encontrado.";
        }

        string senhaAuth = Util.GerarHash(dto.Senha + "-" + usuario.Salt);

        // Verificar a senha do usuário
        if (usuario.Senha != senhaAuth)
        {
            return "Login ou senha incorretos.";
        }

        // Obter lista de IDs de grupos aos quais o usuário pertence
        var gruposIds = _context.UsuarioGrupos
            .Where(gu => gu.UsuarioId == usuario.Id)
            .Select(gu => gu.GrupoId)
            .ToList();

        List<string> stringList = gruposIds.ConvertAll(i => i.ToString());

        // Obter lista de grupos aos quais o usuário pertence
        var grupos = _context.UsuarioGrupos
            .Where(gu => gu.UsuarioId == usuario.Id)
            .Select(gu => gu.Grupo)
            .ToList();

        var permissoes = new GrupoPermissaoDTO
        {
            PodeCriar = grupos.Any(g => g.PodeCriar),
            PodeLer = grupos.Any(g => g.PodeLer),
            PodeAlterar = grupos.Any(g => g.PodeAlterar),
            PodeDeletar = grupos.Any(g => g.PodeDeletar)
        };

        // Gerar token usando TokenService
        return this.GenerateToken(usuario.Id.ToString(), usuario.Login, stringList, permissoes);
    }

    /// <summary>
    /// Gera um token JWT para o usuário e grupos especificados.
    /// </summary>
    /// <param name="userId">O ID do usuário.</param>
    /// <param name="userLogin">O nome do usuário.</param>
    /// <param name="groupIds">A lista de IDs de grupos aos quais o usuário pertence.</param>
    /// <returns>Um token JWT como uma string.</returns>
    private string GenerateToken(string userId, string userLogin, List<string> groupIds, GrupoPermissaoDTO permissao)
    {
        var claims = new List<Claim>
            {
                new Claim("id",userId),
                new Claim("usuario", userId),
                new Claim("loginTimestamp", DateTime.UtcNow.ToString()),
                new Claim("podeLer", permissao.PodeLer.ToString()),
                new Claim("podeCriar", permissao.PodeCriar.ToString()),
                new Claim("podeAlterar", permissao.PodeAlterar.ToString()),
                new Claim("podeAlterar",permissao.PodeDeletar.ToString())
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
