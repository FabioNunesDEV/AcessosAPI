using Acessos.Data;
using Acessos.DTO.Grupo;
using Acessos.Models;
using Acessos.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;

namespace Acessos.Controllers;

[ApiController]
[Route("api/v1/grupos")]
public class GruposController : ControllerBase
{
    private readonly AcessoApiContext _context;
    private readonly IMapper _mapper;

    public GruposController(AcessoApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Cria um novo registro de grupo
    /// </summary>
    /// <param name="grupoDTO">Objeto DTO com informações do grupo</param>
    [HttpPost]
    [ProducesResponseType(typeof(Grupo), (int)HttpStatusCode.Created)]
    public IActionResult PostGrupo([FromBody] GrupoCreateDTO grupoDTO)
    {
        Grupo grupo = _mapper.Map<Grupo>(grupoDTO);
        _context.Grupos.Add(grupo);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetGrupoPorId), new { id = grupo.Id }, grupo);
    }

    /// <summary>
    /// Recupera infromações de um grupo informando o Id
    /// </summary>
    /// <param name="id">Id do grupo</param>
    [HttpGet("{id}")]
    public IActionResult GetGrupoPorId([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("O Id deve ser um número maior que zero.");
        }

        var grupo = _context.Grupos.FirstOrDefault(grupo => grupo.Id == id);
        if (grupo == null) return NotFound();
        var grupoReadDTO = _mapper.Map<GrupoReadDTO>(grupo);
        return Ok(grupoReadDTO);
    }

    /// <summary>
    /// Retorna relação de usuários com grupo informando Id
    /// </summary>
    /// <param name="id">Id do grupo</param>
    [HttpGet("{id}/usuarios")]
    public IActionResult GetGrupoPorIdUsuarios([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("O Id deve ser um número maior que zero.");
        }

        var grupo = _context.Grupos
            .Include(g => g.UsuarioGrupos)
                .ThenInclude(ug => ug.Usuario)
            .FirstOrDefault(g => g.Id == id);

        if (grupo == null) return NotFound();

        var response = new
        {
            Id = grupo.Id,
            Nome = grupo.Nome,
            Usuarios = grupo.UsuarioGrupos.Select(ug => new
            {
                Id = ug.Usuario.Id,
                Nome = ug.Usuario.Nome
            }).ToList()
        };

        return Ok(response);
    }

    /// <summary>
    /// Retorna lista de usuários paginada.
    /// </summary>
    /// <param name="skip">Posição inicial</param>
    /// <param name="take">Quantos regstros serão obtidos a partir da posição inicial</param>
    /// <returns>Retorna lista de grupos</returns>
    [HttpGet]
    public IEnumerable<GrupoReadDTO> GetLista([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var grupos = _mapper.Map<List<GrupoReadDTO>>(_context.Grupos.Skip(skip).Take(take));
        return grupos;
    }

    /// <summary>
    /// Atualiza registro de grupo.
    /// </summary>
    /// <param name="id">Id do grupo</param>
    /// <param name="grupoDTO">Objeto DTO do grupo</param>
    [HttpPut("{id}")]
    public IActionResult PutGrupo(int id, [FromBody] GrupoUpdateDTO grupoDTO)
    {
        var grupoOld = _context.Grupos.FirstOrDefault(grupo => grupo.Id == id);

        if (grupoOld == null) return NotFound();

        _mapper.Map(grupoDTO, grupoOld);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PatchGrupo(int id, [FromBody] JsonPatchDocument<GrupoUpdateDTO> patchDoc)
    {
        var grupo = _context.Grupos.FirstOrDefault(grupo => grupo.Id == id);

        if (grupo == null) return NotFound();

        var grupoDTO = _mapper.Map<GrupoUpdateDTO>(grupo);

        patchDoc.ApplyTo(grupoDTO, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _mapper.Map(grupoDTO, grupo);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Deleta um grupo específico informando seu id.
    /// </summary>
    /// <param name="id">Id do grupo</param>
    [HttpDelete("{id}")]
    public IActionResult DeleteGrupo(int id)
    {
        if (id <= 0)
        {
            return BadRequest("O Id do grupo deve ser maior que zero.");
        }

        var grupo = _context.Grupos.FirstOrDefault(c => c.Id == id);

        if (grupo == null)
        {
            return NotFound("Grupo não encontrado.");
        }

        _context.Grupos.Remove(grupo);
        _context.SaveChanges();

        return NoContent();
    }
}
