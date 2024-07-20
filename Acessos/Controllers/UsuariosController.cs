using Acessos.Data;
using Acessos.DTO.usuario;
using Acessos.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Acessos.Controllers;

[ApiController]
[Route("api/v1/usuarios")]
public class UsuariosController: ControllerBase
{
    private AcessoApiContext _context;
    private IMapper _mapper;

    public UsuariosController (AcessoApiContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Cria um novo registro na tabela Usuario.
    /// </summary>
    /// <param name="usuarioDTO">Objeto DTO com informações do usuário</param>
    [HttpPost]
    [ProducesResponseType(typeof(Usuario), (int)HttpStatusCode.Created)]
    public IActionResult PostUsuario([FromBody] UsuarioCreateDTO usuarioDTO)
    {
        Usuario usuario = _mapper.Map<Usuario>(usuarioDTO);
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetUsuarioPorId), new { id = usuario.Id }, usuario);
    }

    /// <summary>
    /// Recuperar informação de usuário pelo seu id
    /// </summary>
    /// <param name="id">Id do usuário.</param>
    [HttpGet("{id}")]
    public IActionResult GetUsuarioPorId([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("O Id deve ser um número maior que zero.");
        }

        // Modifique esta linha para incluir o carregamento adiantado das relações
        var usuario = _context.Usuarios
                      .Include(u => u.UsuarioGrupos)
                        .ThenInclude(ug => ug.Grupo)                       
                      .FirstOrDefault(u => u.Id == id);

        if (usuario == null) return NotFound($"Usuário com Id {id} não encontrado.");

        var response = new
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Grupos = usuario.UsuarioGrupos.Select(ug => new
            {
                Id = ug.Grupo.Id,
                Nome = ug.Grupo.Nome
            }).ToList()
        };

        return Ok(response);
    }

    /// <summary>
    /// Recupera uma listagem de usuários paginada.
    /// </summary>
    /// <param name="skip">Posição inicial</param>
    /// <param name="take">Quantos regstros serão obtidos a partir da posição inicial</param>
    /// <returns>Retorna lista de usuários</returns>
    [HttpGet]
    public IActionResult GetLista([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {

        var usuarios = _mapper.Map<List<UsuarioReadDTO>>(_context.Usuarios.Skip(skip).Take(take)); 

        return Ok(usuarios);
    }

    /// <summary>
    /// Atualiza registro de usuario.
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <param name="usuarioDTO">Objeto DTO do usuario</param>
    [HttpPut("{id}")]
    public IActionResult PutUsuario(int id, [FromBody] UsuarioUpdateDTO usuarioDTO)
    {
        if (id <= 0)
        {
            return BadRequest("O Id deve ser um numero maior que zero");
        }

        var usuarioOld = _context.Usuarios.FirstOrDefault(usuario=>usuario.Id == id);

        if (usuarioOld == null) return NotFound($"Usuário com Id {id} não encontrado.");

        _mapper.Map(usuarioDTO, usuarioOld);
        _context.SaveChanges();

        return NoContent();
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
        var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);

        if (usuario == null) return NotFound();

        var usuarioDTO = _mapper.Map<UsuarioUpdateDTO>(usuario);


        patchDoc.ApplyTo(usuarioDTO, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(usuarioDTO, usuario);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Deleta um usuario específico por seu id.
    /// </summary>
    /// <param name="id">Id do usuário</param>
    [HttpDelete("{id}")]
    public IActionResult DeleteUsuario(int id)
    {
        if (id <= 0)
        {
            return BadRequest("O Id do usuario deve ser maior que zero.");
        }

        Usuario usuario = _context.Usuarios.FirstOrDefault(c => c.Id == id);

        if (usuario == null)
        {
            return NotFound($"Usuário com Id {id} não encontrado.");
        }

        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Adiciona um novo relacionamento entre usuário e grupo.
    /// </summary>
    /// <param name="usuarioId">Id do usuário</param>
    /// <param name="grupoId">Id do grupo</param>
    [HttpPost("{usuarioId}/grupos/{grupoId}")]
    public IActionResult PostUsuarioGrupo([FromRoute] int usuarioId, [FromRoute] int grupoId)
    {
        // Verifica se o usuário existe
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
        if (usuario == null)
        {
            return NotFound($"Usuário com Id {usuarioId} não encontrado.");
        }

        // Verifica se o grupo existe
        var grupo = _context.Grupos.FirstOrDefault(g => g.Id == grupoId);
        if (grupo == null)
        {
            return NotFound($"Grupo com Id {grupoId} não encontrado.");
        }

        // Verifica se o relacionamento já existe
        var usuarioGrupoExistente = _context.UsuarioGrupos.FirstOrDefault(ug => ug.UsuarioId == usuarioId && ug.GrupoId == grupoId);
        if (usuarioGrupoExistente != null)
        {
            return BadRequest("Este usuário já está associado a este grupo.");
        }

        // Cria o novo relacionamento
        var usuarioGrupo = new UsuarioGrupo
        {
            UsuarioId = usuarioId,
            GrupoId = grupoId
        };

        _context.UsuarioGrupos.Add(usuarioGrupo);
        _context.SaveChanges();

        return Ok();
    }

    /// <summary>
    /// Cria um relacionamento entre usuário e grupo.
    /// </summary>
    /// <param name="usuarioId">Id do usuário</param>
    /// <param name="grupoId">Id do grupo</param>
    [HttpDelete("{usuarioId}/grupos/{grupoId}")]
    public IActionResult DeleteUsuarioGrupo([FromRoute] int usuarioId, [FromRoute] int grupoId)
    {
        // Verifica se o usuário existe
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
        if (usuario == null)
        {
            return NotFound($"Usuário com Id {usuarioId} não encontrado.");
        }

        // Verifica se o grupo existe
        var grupo = _context.Grupos.FirstOrDefault(g => g.Id == grupoId);
        if (grupo == null)
        {
            return NotFound($"Grupo com Id {grupoId} não encontrado.");
        }

        // Verifica se o relacionamento já existe
        var usuarioGrupoExistente = _context.UsuarioGrupos.FirstOrDefault(ug => ug.UsuarioId == usuarioId && ug.GrupoId == grupoId);
        if (usuarioGrupoExistente == null)
        {
            return BadRequest("Este usuário não está associado a este grupo.");
        }

        _context.UsuarioGrupos.Remove(usuarioGrupoExistente);
        _context.SaveChanges();

        return Ok();
    }
}
