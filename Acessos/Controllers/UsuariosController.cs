using Acessos.Data;
using Acessos.DTO;
using Acessos.Models;
using Acessos.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Criar([FromBody] UsuarioCreateDTO usuarioDTO)
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

        var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
        if (usuario == null) return NotFound();
        var usuarioReadDTO = _mapper.Map<UsuarioReadDTO>(usuario);
        return Ok(usuarioReadDTO);
    }

    /// <summary>
    /// Recupera uma listagem de usuários paginada.
    /// </summary>
    /// <param name="skip">Posição inicial</param>
    /// <param name="take">Quantos regstros serão obtidos a partir da posição inicial</param>
    /// <returns>Retorna uma coleção de documentos</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<UsuarioReadDTO> GetLista([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return _mapper.Map<List<UsuarioReadDTO>>(_context.Usuarios.Skip(skip).Take(take));
    }

    /// <summary>
    /// Atualiza registro de usuario.
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <param name="usuario"></param>
    /// <returns></returns>
    [HttpPut("usuario/{id}")]
    public IActionResult Atualizar(int id, [FromBody] UsuarioUpdateDTO usuario)
    {
        var usuarioOld = _context.Usuarios.FirstOrDefault(usuario=>usuario.Id == id);

        if (usuarioOld == null) return NotFound();

        _mapper.Map(usuario, usuarioOld);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="patchDoc"></param>
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
    /// <returns></returns>
    [HttpPatch("usuario/{id}")]
    public IActionResult AtualizarParcial(int id, [FromBody] JsonPatchDocument<UsuarioUpdateDTO> patchDoc) 
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

}
