using Acessos.DTO.usuario;
using Acessos.Exceptions;
using Acessos.Models;
using Acessos.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Acessos.Controllers;

[ApiController]
[Route("api/v1/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly UsuariosService _usuariosService;

    public UsuariosController(UsuariosService usuariosService)
    {
        _usuariosService = usuariosService;
    }

    /// <summary>
    /// Cria um novo registro na tabela Usuario.
    /// </summary>
    /// <param name="usuarioDTO">Objeto DTO com informações do usuário</param>
    [HttpPost]
    [ProducesResponseType(typeof(Usuario), (int)HttpStatusCode.Created)]
    public IActionResult PostUsuario([FromBody] UsuarioCreateDTO usuarioDTO)
    {
        return Requisicao.Manipulador(() =>
        {
            var usuario = _usuariosService.CadastrarUsuario(usuarioDTO);
            return CreatedAtAction(nameof(GetUsuarioPorId), new { id = usuario.Id }, usuario);
        });
    }

    /// <summary>
    /// Recuperar informação de usuário pelo seu id
    /// </summary>
    /// <param name="id">Id do usuário.</param>
    [HttpGet("{id}")]
    public IActionResult GetUsuarioPorId([FromRoute] int id)
    {
        return Requisicao.Manipulador(() =>
        {
            var usuario = _usuariosService.ObterUsuarioPorId(id);
            return Ok(usuario);
        });
    }

    /// <summary>
    /// Recuperar informação de usuário e seus grupos relacionados pelo id de usuário
    /// </summary>
    /// <param name="id">Id do usuário.</param>
    [HttpGet("{id}/grupo")]
    public IActionResult GetUsuarioPorIdGrupos([FromRoute] int id)
    {
        return Requisicao.Manipulador(() =>
        {
            var usuario = _usuariosService.ObterUsuarioPorIdGrupos(id);
            return Ok(usuario);
        });
    }

    /// <summary>
    /// Recuperar informação de usuário pelo seu login
    /// </summary>
    /// <param name="login">Login do usuário.</param>
    [HttpGet("login/{login}")]
    public IActionResult GetUsuarioPorLogin([FromRoute] string login)
    {
        return Requisicao.Manipulador(() =>
        {
            var usuario = _usuariosService.ObterUsuarioPorLogin(login);
            return Ok(usuario);
        });
    }

    /// <summary>
    /// Recupera uma listagem de usuários paginada.
    /// </summary>
    /// <param name="skip">Posição inicial</param>
    /// <param name="take">Quantos registros serão obtidos a partir da posição inicial</param>
    /// <returns>Retorna lista de usuários</returns>
    [HttpGet]
    public IActionResult GetLista([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return Requisicao.Manipulador(() =>
        {
            var usuarios = _usuariosService.ObterListaUsuarios(skip, take);
            return Ok(usuarios);
        });
    }

    /// <summary>
    /// Atualiza registro de usuario.
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <param name="usuarioDTO">Objeto DTO do usuario</param>
    [HttpPut("{id}")]
    public IActionResult PutUsuario(int id, [FromBody] UsuarioUpdateDTO usuarioDTO)
    {
        return Requisicao.Manipulador(() =>
        {
            _usuariosService.AtualizarUsuario(id, usuarioDTO);
            return NoContent();
        });
    }

    /// <summary>
    /// Altera parcialmente as informações de um usuário informando seu id.
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <param name="patchDoc">Array Json com as informações de alteração</param>
    /// <remarks>
    /// Exemplo de uso:
    ///
    ///     Patch 
    ///     [
    ///         { "op": "replace", "path": "/nome", "value": "NovoNome" },
    ///         { "op": "replace", "path": "/email", "value": "NovoEmail" }
    ///     ]
    ///
    /// </remarks>
    [HttpPatch("{id}")]
    public IActionResult PatchUsuario(int id, [FromBody] JsonPatchDocument<UsuarioUpdateDTO> patchDoc)
    {
        return Requisicao.Manipulador(() =>
        {
            _usuariosService.AtualizarUsuarioParcialmente(id, patchDoc);
            return NoContent();
        });
    }

    /// <summary>
    /// Deleta um usuario específico por seu id.
    /// </summary>
    /// <param name="id">Id do usuário</param>
    [HttpDelete("{id}")]
    public IActionResult DeleteUsuario(int id)
    {
        return Requisicao.Manipulador(() =>
        {
            _usuariosService.DeletarUsuario(id);
            return NoContent();
        });
    }

    /// <summary>
    /// Adiciona um novo relacionamento entre usuário e grupo.
    /// </summary>
    /// <param name="usuarioId">Id do usuário</param>
    /// <param name="grupoId">Id do grupo</param>
    [HttpPost("{usuarioId}/grupos/{grupoId}")]
    public IActionResult PostUsuarioGrupo([FromRoute] int usuarioId, [FromRoute] int grupoId)
    {
        return Requisicao.Manipulador(() =>
        {
            _usuariosService.AdicionarUsuarioGrupo(usuarioId, grupoId);
            return Ok();
        });
    }

    /// <summary>
    /// Remove um relacionamento entre usuário e grupo.
    /// </summary>
    /// <param name="usuarioId">Id do usuário</param>
    /// <param name="grupoId">Id do grupo</param>
    [HttpDelete("{usuarioId}/grupos/{grupoId}")]
    public IActionResult DeleteUsuarioGrupo([FromRoute] int usuarioId, [FromRoute] int grupoId)
    {
        return Requisicao.Manipulador(() =>
        {
            _usuariosService.RemoverUsuarioGrupo(usuarioId, grupoId);
            return Ok();
        });
    }
}
