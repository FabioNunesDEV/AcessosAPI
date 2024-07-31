using Acessos.Models;
using Acessos.DTO.Circular;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Acessos.Data;
using System.Net;
using Acessos.Services;

namespace Acessos.Controllers
{
    [ApiController]
    [Route("api/v1/circulares")]
    public class CircularesController : ControllerBase
    {
        private readonly AcessoApiContext _context;
        private readonly IMapper _mapper;
        private readonly CircularesService _circularesService;

        public CircularesController(
            AcessoApiContext context,
            IMapper mapper,
            CircularesService circularesService)
        {
            _context = context;
            _mapper = mapper;
            _circularesService = circularesService;
        }

        /// <summary>
        /// Cria um novo registro para Circular.
        /// </summary>
        /// <param name="circularDTO">Objeto DTO com informa��es da circular.</param>
        /// <returns>
        /// Retorna Created (HTTP 201) com os detalhes da circular criada.
        /// </returns>
        /// <response code="201">Circular criada com sucesso.</response>
        /// <response code="400">O objeto DTO fornecido � inv�lido.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Circular), (int)HttpStatusCode.Created)]
        public IActionResult PostCircular([FromBody] CircularCreateDTO circularDTO)
        {
            return HandleRequest(() =>
            {
                var circular = _circularesService.CadastrarCircular(circularDTO);
                return CreatedAtAction(nameof(GetCircularPorId), new { id = circular.Id }, circular);
            });
        }

        /// <summary>
        /// Retorna uma lista paginada de circulares.
        /// </summary>
        /// <param name="skip">Posi��o inicial para a pagina��o.</param>
        /// <param name="take">N�mero de registros a serem obtidos a partir da posi��o inicial.</param>
        /// <returns>
        /// Retorna Ok (HTTP 200) com a lista de circulares.
        /// </returns>
        /// <response code="200">Lista de circulares retornada com sucesso.</response>
        [HttpGet]
        public IActionResult GetCircularLista([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return HandleRequest(() =>
            {
                var circulares = _circularesService.ObterListaCirculares(skip, take);
                return Ok(circulares);
            });
        }

        /// <summary>
        /// Recupera informa��es de uma circular espec�fica informando seu id.
        /// </summary>
        /// <param name="id">Id da circular a ser recuperada.</param>
        /// <returns>
        /// Retorna Ok (HTTP 200) com os detalhes da circular se encontrada,
        /// NotFound (HTTP 404) se a circular n�o for encontrada,
        /// ou BadRequest (HTTP 400) se o id for inv�lido.
        /// </returns>
        /// <response code="200">Detalhes da circular retornados com sucesso.</response>
        /// <response code="404">Circular com o id especificado n�o foi encontrada.</response>
        /// <response code="400">O id fornecido � inv�lido.</response>
        [HttpGet("{id}")]
        public IActionResult GetCircularPorId([FromRoute] int id)
        {
            return HandleRequest(() => {

                var circular = _circularesService.ObterCircularPorId(id);
                return Ok(circular);

            });
        }

        /// <summary>
        /// Atualiza o registro de uma circular espec�fica informando seu id.
        /// </summary>
        /// <param name="id">Id da circular a ser atualizada.</param>
        /// <param name="circularDTO">Objeto DTO com as novas informa��es da circular.</param>
        /// <returns>
        /// Retorna NoContent (HTTP 204) se a atualiza��o for bem-sucedida, 
        /// NotFound (HTTP 404) se a circular n�o for encontrada, 
        /// ou BadRequest (HTTP 400) se o id for inv�lido.
        /// </returns>
        /// <response code="204">Circular atualizada com sucesso.</response>
        /// <response code="404">Circular com o id especificado n�o foi encontrada.</response>
        /// <response code="400">O id fornecido � inv�lido.</response>
        [HttpPut("{id}")]
        public IActionResult PutCircular(int id, [FromBody] CircularUpdateDTO circularDTO)
        {
            return HandleRequest(() =>
            {
                _circularesService.AtualizarCircular(id, circularDTO);
                return NoContent();
            });
        }

        /// <summary>
        /// Marca uma circular como lida, atualizando seu status e data de recebimento.
        /// </summary>
        /// <param name="id">Id da circular a ser marcada como lida.</param>
        /// <returns>
        /// Retorna NoContent (HTTP 204) se a atualiza��o for bem-sucedida, 
        /// NotFound (HTTP 404) se a circular n�o for encontrada, 
        /// ou BadRequest (HTTP 400) se o id for inv�lido.
        /// </returns>
        /// <response code="204">Circular marcada como lida com sucesso.</response>
        /// <response code="404">Circular com o id especificado n�o foi encontrada.</response>
        /// <response code="400">O id fornecido � inv�lido.</response>
        [HttpPut("{id}/Lida")]
        public IActionResult PutCircularLida(int id)
        {
            return HandleRequest(() =>
            {
                _circularesService.AtualizarComoLida(id);
                return NoContent();
            });
        }

        /// <summary>
        /// Deleta uma circular espec�fica informando seu id.
        /// </summary>
        /// <param name="id">Id da Circular a ser deletada.</param>
        /// <returns>
        /// Retorna NoContent (HTTP 204) se a exclus�o for bem-sucedida,
        /// NotFound (HTTP 404) se a circular n�o for encontrada,
        /// ou BadRequest (HTTP 400) se o id for inv�lido.
        /// </returns>
        /// <response code="204">Circular deletada com sucesso.</response>
        /// <response code="404">Circular com o id especificado n�o foi encontrada.</response>
        /// <response code="400">O id fornecido � inv�lido.</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteCircular([FromRoute] int id)
        {
            return HandleRequest(() =>
            {
                _circularesService.DeletarCircular(id);

                return NoContent();
            });
        }

        /// <summary>
        /// Trata com a execu��o de uma fun��o fornecida e padroniza o tratamento de exce��es.
        /// </summary>
        /// <param name="func">A fun��o a ser executada, que retorna um IActionResult.</param>
        /// <returns>
        /// Retorna o resultado da execu��o da fun��o ou uma resposta de erro apropriada se ocorrer uma exce��o.
        /// </returns>
        /// <remarks>
        /// Este m�todo captura exce��es espec�ficas e retorna respostas HTTP padronizadas:
        /// <list type="bullet">
        /// <item><description>ArgumentException: Retorna um BadRequest (HTTP 400) com a mensagem da exce��o.</description></item>
        /// <item><description>KeyNotFoundException: Retorna um NotFound (HTTP 404) com a mensagem da exce��o.</description></item>
        /// <item><description>Exception: Retorna um StatusCode (HTTP 500) com uma mensagem de erro gen�rica e os detalhes da exce��o.</description></item>
        /// </list>
        /// </remarks>
        private IActionResult HandleRequest(Func<IActionResult> func)
        {
            try
            {
                return func();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro interno no servidor.\n{ex}");
            }
        }
    }
}
