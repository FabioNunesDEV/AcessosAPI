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

namespace Acessos.Services;

/// <summary>
/// Serviço de autenticação para gerenciar tokens JWT.
/// </summary>
public class AuthService
{
    private readonly AcessoApiContext _context;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="AuthService"/>.
    /// </summary>
    /// <param name="context">O contexto do banco de dados.</param>
    /// <param name="tokenService">O serviço de token.</param>
    public AuthService(AcessoApiContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Obtém um token JWT para o usuário especificado.
    /// </summary>
    /// <param name="dto">Objeto AuthReadDTO.</param>
    /// <returns>Um token JWT como uma string ou uma mensagem de erro.</returns>
    public string ObterToken(AuthReadDTO dto)
    {
        var usuario = ObterUsuario(dto.Login);
        if (usuario == null || !VerificarSenha(usuario, dto.Senha))
        {
            throw new KeyNotFoundException("Login ou senha incorretos.");
        }

        var permissoes = ObterPermissoes(usuario.Id);
        var gruposIds = ObterGruposIds(usuario.Id);

        return _tokenService.GenerateToken(usuario.Id.ToString(), usuario.Login, gruposIds, permissoes);
    }

    private Usuario ObterUsuario(string login)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Login == login);
    }

    private bool VerificarSenha(Usuario usuario, string senha)
    {
        string senhaAuth = Util.GerarHash(senha + "-" + usuario.Salt);
        return usuario.Senha == senhaAuth;
    }

    private GrupoPermissaoDTO ObterPermissoes(int usuarioId)
    {
        var grupos = _context.UsuarioGrupos
            .Where(gu => gu.UsuarioId == usuarioId)
            .Select(gu => gu.Grupo)
            .ToList();

        return new GrupoPermissaoDTO
        {
            PodeCriar = grupos.Any(g => g.PodeCriar),
            PodeLer = grupos.Any(g => g.PodeLer),
            PodeAlterar = grupos.Any(g => g.PodeAlterar),
            PodeDeletar = grupos.Any(g => g.PodeDeletar)
        };
    }

    private List<string> ObterGruposIds(int usuarioId)
    {
        var gruposIds = _context.UsuarioGrupos
            .Where(gu => gu.UsuarioId == usuarioId)
            .Select(gu => gu.GrupoId)
            .ToList();

        return gruposIds.ConvertAll(i => i.ToString());
    }
}
