using Acessos.Models;
using Acessos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Acessos.Controllers;

[ApiController]
[Route("api/v1/documentos")]
public class DocumentosController : ControllerBase
{
    private readonly CircularService _circularService;

    public DocumentosController(CircularService circularService)
    {
        _circularService = circularService;
    }

    /// <summary>
    /// Retorna coleção com todos os documentos cadastrados.
    /// </summary>
    /// <response code="200">Retorna os documentos com sucesso</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult RetornarTodosDocumentos()
    {
        return Ok(_circularService.circulares);
    }

    /// <summary>
    /// Inseri um novo documento.
    /// </summary>
    /// <response code="200">Retorna o documento com sucesso</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult InserirDocumento([FromBody] Circular circular)
    {
        int id = _circularService.IncrementarId();
        circular.Id = id;

        _circularService.circulares.Add(circular);

        return Ok(circular);
    }

    /// <summary>
    /// Retorna um documento específico por ID.
    /// </summary>
    /// <param name="id">Id do documento</param>
    /// <response code="200">Retorna o documento deletado com sucesso</response>
    /// <response code="400">Se o Id não for informado</response>
    /// <response code="404">Caso nenhum documento for encontrado</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpGet("documento/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult RetornarDocumentoPorId([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("O Id do documento deve ser maior que zero.");
        }

        Circular circular = _circularService.circulares.FirstOrDefault(c => c.Id == id);

        if (circular == null)
        {
            return NotFound("Documento não encontrado.");
        }

        return Ok(circular);
    }

    /// <summary>
    /// Altera as informações de um documento informando seu id.
    /// </summary>
    /// <param name="id">Id do documento</param>
    /// <response code="200">Retorna o documento com sucesso</response>
    /// <response code="400">Se o Id não for informado</response>
    /// <response code="404">Caso nenhum documento for encontrado</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpPut("documento{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Alterar()
    {
        throw new ApplicationException("Método não concluido");
    }

    /// <summary>
    /// Deleta um documento específico por Id.
    /// </summary>
    /// <param name="id">Id do documento</param>
    /// <response code="200">Retorna o documento deletado com sucesso</response>
    /// <response code="400">Se o Id não for informado</response>
    /// <response code="404">Caso nenhum documento for encontrado</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpDelete("documento/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeletarPorId([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("O Id do documento deve ser maior que zero.");
        }

        Circular circular = _circularService.circulares.FirstOrDefault(c => c.Id == id);

        if (circular == null)
        {
            return NotFound("Documento não encontrado.");
        }

        _circularService.circulares.Remove(circular);

        return Ok(circular);
    }

    /// <summary>
    /// Retorna um documento específico por seu número de protocolo.
    /// </summary>
    /// <param name="protocolo">Protocolo do documento</param>
    /// <response code="200">Retorna o documento com sucesso</response>
    /// <response code="400">Se o protocolo não for informado</response>
    /// <response code="404">Caso nenhum documento for encontrado</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpGet("protocolo/{protocolo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult RetornarDocumentoPorProtocolo([FromRoute] string protocolo)
    {
        if (string.IsNullOrWhiteSpace(protocolo))
        {
            return BadRequest("O Protocolo do documento deve ser informado.");
        }

        Circular circular = _circularService.circulares.FirstOrDefault(c => c.Protocolo == protocolo);

        if (circular == null)
        {
            return NotFound("Documento não encontrado.");
        }

        return Ok(circular);
    }

    /// <summary>
    /// Deleta um documento específico por seu protocolo.
    /// </summary>
    /// <param name="protocolo">Protocolo do documento</param>
    /// <response code="200">Retorna o documento deletado com sucesso</response>
    /// <response code="400">Se o protocolo não for informado</response>
    /// <response code="404">Caso nenhum protocolo for encontrado</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpDelete("protocolo/{protocolo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeletarPorProtocolo([FromRoute] string protocolo)
    {
        if (string.IsNullOrWhiteSpace(protocolo))
        {
            return BadRequest("O Protocolo do documento deve ser informado.");
        }

        Circular circular = _circularService.circulares.FirstOrDefault(c => c.Protocolo == protocolo);

        if (circular == null)
        {
            return NotFound("Documento não encontrado.");
        }

        _circularService.circulares.Remove(circular);

        return Ok(circular);
    }
}
