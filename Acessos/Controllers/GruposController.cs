using Acessos.Data;
using Acessos.DTO.Grupo;
using Acessos.Exceptions;
using Acessos.Models;
using Acessos.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Acessos.Controllers;

[ApiController]
[Route("api/v1/grupos")]
public class GruposController : ControllerBase
{
    private readonly GruposService _gruposService;

    public GruposController(GruposService gruposService)
    {
        _gruposService = gruposService;
    }

    /// <summary>
    /// Cria um novo registro de grupo
    /// </summary>
    /// <param name="grupoDTO">Objeto DTO com informações do grupo</param>
    [HttpPost]
    [ProducesResponseType(typeof(Grupo), (int)HttpStatusCode.Created)]
    public IActionResult PostGrupo([FromBody] GrupoCreateDTO grupoDTO)
    {
        return Requisicao.Manipulador(() =>
        {
            var grupo = _gruposService.CadastrarGrupo(grupoDTO);
            return CreatedAtAction(nameof(GetGrupoPorId), new { id = grupo.Id }, grupo);
        });
    }

    /// <summary>
    /// Recupera informações de um grupo informando o Id
    /// </summary>
    /// <param name="id">Id do grupo</param>
    [HttpGet("{id}")]
    public IActionResult GetGrupoPorId([FromRoute] int id)
    {
        return Requisicao.Manipulador(() =>
        {
            var grupo = _gruposService.ObterGrupoPorId(id);
            return Ok(grupo);
        });
    }

    /// <summary>
    /// Retorna relação de usuários com grupo informando Id
    /// </summary>
    /// <param name="id">Id do grupo</param>
    [HttpGet("{id}/usuarios")]
    public IActionResult GetGrupoPorIdUsuarios([FromRoute] int id)
    {
        return Requisicao.Manipulador(() =>
        {
            var grupo = _gruposService.ObterGrupoPorId(id);
            return Ok(grupo);
        });
    }

    /// <summary>
    /// Retorna lista de grupos paginada.
    /// </summary>
    /// <param name="skip">Posição inicial</param>
    /// <param name="take">Quantos registros serão obtidos a partir da posição inicial</param>
    /// <returns>Retorna lista de grupos</returns>
    [HttpGet]
    public IActionResult GetLista([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return Requisicao.Manipulador(() =>
        {
            var grupos = _gruposService.ObterListaGrupos(skip, take);
            return Ok(grupos);
        });
    }

    /// <summary>
    /// Atualiza registro de grupo.
    /// </summary>
    /// <param name="id">Id do grupo</param>
    /// <param name="grupoDTO">Objeto DTO do grupo</param>
    [HttpPut("{id}")]
    public IActionResult PutGrupo(int id, [FromBody] GrupoUpdateDTO grupoDTO)
    {
        return Requisicao.Manipulador(() =>
        {
            _gruposService.AtualizarGrupo(id, grupoDTO);
            return NoContent();
        });
    }

    /// <summary>
    /// Altera parcialmente as informações de um grupo informando seu Id.
    /// </summary>
    /// <param name="id">Id do Grupo</param>
    /// <param name="patchDoc">Array Json com as informações de alteração</param>
    /// <remarks>
    /// Exemplo de uso:
    ///
    ///     Patch 
    ///     [
    ///         { "op": "replace", "path": "/nome", "value": "NovoNome" },
    ///     ]
    ///
    /// </remarks>
    [HttpPatch("{id}")]
    public IActionResult PatchGrupo(int id, [FromBody] JsonPatchDocument<GrupoUpdateDTO> patchDoc)
    {
        return Requisicao.Manipulador(() =>
        {
            _gruposService.AtualizarGrupoParcialmente(id, patchDoc);
            return NoContent();
        });
    }

    /// <summary>
    /// Deleta um grupo específico informando seu id.
    /// </summary>
    /// <param name="id">Id do grupo</param>
    [HttpDelete("{id}")]
    public IActionResult DeleteGrupo(int id)
    {
        return Requisicao.Manipulador(() =>
        {
            _gruposService.DeletarGrupo(id);
            return NoContent();
        });
    }
}
