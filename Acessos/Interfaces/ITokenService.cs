using Acessos.DTO.Grupo;

namespace Acessos.Interfaces;

/// <summary>
/// Interface para o serviço de token.
/// </summary>
public interface ITokenService
{
    string GenerateToken(string userId, string userLogin, List<string> groupIds, GrupoPermissaoDTO permissao);
}
