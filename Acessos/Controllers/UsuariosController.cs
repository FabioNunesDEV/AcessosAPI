using Acessos.Data;
using Acessos.DTO;
using Acessos.Models;
using Acessos.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
    /// 
    /// </summary>
    /// <param name="usuarioDTO"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Adicionar([FromBody] UsuarioCreateDTO usuarioDTO)
    {
        Usuario usuario = _mapper.Map<Usuario>(usuarioDTO);
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperarPorId), new { id = usuario.Id }, usuario);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("usuario/{id}")]
    public IActionResult RecuperarPorId(int id) 
    {
        var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
        if (usuario == null) return NotFound();
        var usuarioReadDTO = _mapper.Map<UsuarioReadDTO>(usuario);
        return Ok(usuarioReadDTO);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("lista")]
    public IEnumerable<UsuarioReadDTO> RecuperarLista()
    {
        return _mapper.Map<List<UsuarioReadDTO>>(_context.Usuarios);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="usuarioDTO"></param>
    /// <returns></returns>
    [HttpPut("usuario/{id}")]
    public IActionResult Atualizar(int id, [FromBody] UsuarioUpdateDTO usuarioDTO)
    {
        var usuario = _context.Usuarios.FirstOrDefault(usuario=>usuario.Id == id);

        if (usuario == null) return NotFound();

        _mapper.Map(usuarioDTO, usuario);
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
