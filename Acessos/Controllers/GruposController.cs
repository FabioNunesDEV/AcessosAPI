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

    [HttpPost]
    [ProducesResponseType(typeof(Grupo), (int)HttpStatusCode.Created)]
    public IActionResult PostGrupo([FromBody] GrupoCreateDTO grupoDTO)
    {
        Grupo grupo = _mapper.Map<Grupo>(grupoDTO);
        _context.Grupos.Add(grupo);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetGrupoPorId), new { id = grupo.Id }, grupo);
    }

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

    [HttpGet]
    public IEnumerable<GrupoReadDTO> GetLista([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return _mapper.Map<List<GrupoReadDTO>>(_context.Grupos.Skip(skip).Take(take));
    }

    [HttpPut("grupo/{id}")]
    public IActionResult PutGrupo(int id, [FromBody] GrupoUpdateDTO grupoDTO)
    {
        var grupoOld = _context.Grupos.FirstOrDefault(grupo => grupo.Id == id);

        if (grupoOld == null) return NotFound();

        _mapper.Map(grupoDTO, grupoOld);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("grupo/{id}")]
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

    [HttpDelete("grupo/{id}")]
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
