﻿using Acessos.Data;
using Acessos.DTO.Auth;
using Acessos.DTO.Grupo;
using Acessos.Models;
using Acessos.Utilities;

namespace Acessos.Services;

/// <summary>
/// Serviço de autenticação para gerenciar tokens JWT.
/// </summary>
public class AuthService
{
    private readonly AcessoApiContext _context;
    private readonly TokenService _tokenService;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="AuthService"/>.
    /// </summary>
    /// <param name="context">O contexto do banco de dados.</param>
    /// <param name="tokenService">O serviço de token.</param>
    public AuthService(AcessoApiContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Obtém um token JWT para o usuário especificado.
    /// </summary>
    /// <param name="dto">Objeto AuthReadDTO.</param>
    /// <returns>Um token JWT como uma string ou uma mensagem de erro.</returns>
    public string ObterUsuario(AuthReadDTO dto)
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

        var grupoPermissaoDTO = new GrupoPermissaoDTO
        {
            PodeCriar = grupos.Any(g => g.PodeCriar),
            PodeLer = grupos.Any(g => g.PodeLer),
            PodeAlterar = grupos.Any(g => g.PodeAlterar),
            PodeDeletar = grupos.Any(g => g.PodeDeletar)
        };

        // Gerar token usando TokenService
        return _tokenService.GenerateToken(usuario.Id.ToString(), usuario.Login, stringList);
    }
}
