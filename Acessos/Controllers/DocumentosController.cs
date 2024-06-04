using Acessos.Data;
using Acessos.Models;
using Microsoft.AspNetCore.Mvc;

namespace Acessos.Controllers;

[ApiController]
[Route("api/v1/documentos")]
public class DocumentosController : ControllerBase
{
    CircularData _circularData;
    List<Circular> _circulares;

    public DocumentosController()
    {
        _circularData = new CircularData();
        _circulares = _circularData.GetDadosCirculares();
    }

    /// <summary>
    /// Retorna coleção com todos os documentos cadastrados.
    /// </summary>
    /// <response code="200">Retorna os documentos com sucesso</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpGet ("todos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult RetornarDocumentos()
    {
        throw new ApplicationException("Método não concluido");
    }

    /// <summary>
    /// Retorna um documento específico por ID.
    /// </summary>
    /// <param name="documentoId">Id do documento</param>
    /// <response code="200">Retorna o documento com sucesso</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpGet ("{documentoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult PorId([FromRoute]int documentoId)
    {
        Circular circular = _circulares.FirstOrDefault(c => c.Id == documentoId);

        return Ok(circular);
    }

    /// <summary>
    /// Inseri um novo documento.
    /// </summary>
    /// <response code="200">Retorna o documento com sucesso</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Criar()
    {
        throw new ApplicationException("Método não concluido");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="documentoId">Id do documento</param>
    /// <response code="200">Retorna o documento com sucesso</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpPut("{documentoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Alterar()
    {
        throw new ApplicationException("Método não concluido");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="documentoId">Id do documento</param>
    /// <response code="200">Retorna o documento com sucesso</response>
    /// <response code="500">Se houver um erro interno no servidor</response>
    [HttpDelete ("{documentoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Deletar(int id)
    {
        throw new ApplicationException("Método não concluido");
    }
}
