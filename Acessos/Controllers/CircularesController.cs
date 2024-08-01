using Acessos.Models;
using Acessos.DTO.Circular;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Acessos.Data;
using System.Net;
using Acessos.Services;
using Acessos.Exceptions;

namespace Acessos.Controllers
{
    [ApiController]
    [Route("api/v1/circulares")]
    public class CircularesController : ControllerBase
    {

        private readonly CircularesService _circularesService;

        public CircularesController(
            AcessoApiContext context,
            IMapper mapper,
            CircularesService circularesService)
        {
            _circularesService = circularesService;
        }

        /// <summary>
        /// Cria um novo registro para Circular.
        /// </summary>
        /// <param name="circularDTO">Objeto DTO com informações da circular.</param>
        /// <returns>
        /// Retorna Created (HTTP 201) com os detalhes da circular criada.
        /// </returns>
        /// <response code="201">Circular criada com sucesso.</response>
        /// <response code="400">O objeto DTO fornecido é inválido.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Circular), (int)HttpStatusCode.Created)]
        public IActionResult PostCircular([FromBody] CircularCreateDTO circularDTO)
        {
            return Requisicao.Manipulador(() =>
            {
                var circular = _circularesService.CadastrarCircular(circularDTO);
                return CreatedAtAction(nameof(GetCircularPorId), new { id = circular.Id }, circular);
            });
        }

        /// <summary>
        /// Retorna uma lista paginada de circulares.
        /// </summary>
        /// <param name="skip">Posição inicial para a paginação.</param>
        /// <param name="take">Número de registros a serem obtidos a partir da posição inicial.</param>
        /// <returns>
        /// Retorna Ok (HTTP 200) com a lista de circulares.
        /// </returns>
        /// <response code="200">Lista de circulares retornada com sucesso.</response>
        [HttpGet]
        public IActionResult GetCircularLista([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Requisicao.Manipulador(() =>
            {
                var circulares = _circularesService.ObterListaCirculares(skip, take);
                return Ok(circulares);
            });
        }

        /// <summary>
        /// Recupera informações de uma circular específica informando seu id.
        /// </summary>
        /// <param name="id">Id da circular a ser recuperada.</param>
        /// <returns>
        /// Retorna Ok (HTTP 200) com os detalhes da circular se encontrada,
        /// NotFound (HTTP 404) se a circular não for encontrada,
        /// ou BadRequest (HTTP 400) se o id for inválido.
        /// </returns>
        /// <response code="200">Detalhes da circular retornados com sucesso.</response>
        /// <response code="404">Circular com o id especificado não foi encontrada.</response>
        /// <response code="400">O id fornecido é inválido.</response>
        [HttpGet("{id}")]
        public IActionResult GetCircularPorId([FromRoute] int id)
        {
            return Requisicao.Manipulador(() => {

                var circular = _circularesService.ObterCircularPorId(id);
                return Ok(circular);

            });
        }

        /// <summary>
        /// Atualiza o registro de uma circular específica informando seu id.
        /// </summary>
        /// <param name="id">Id da circular a ser atualizada.</param>
        /// <param name="circularDTO">Objeto DTO com as novas informações da circular.</param>
        /// <returns>
        /// Retorna NoContent (HTTP 204) se a atualização for bem-sucedida, 
        /// NotFound (HTTP 404) se a circular não for encontrada, 
        /// ou BadRequest (HTTP 400) se o id for inválido.
        /// </returns>
        /// <response code="204">Circular atualizada com sucesso.</response>
        /// <response code="404">Circular com o id especificado não foi encontrada.</response>
        /// <response code="400">O id fornecido é inválido.</response>
        [HttpPut("{id}")]
        public IActionResult PutCircular(int id, [FromBody] CircularUpdateDTO circularDTO)
        {
            return Requisicao.Manipulador(() =>
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
        /// Retorna NoContent (HTTP 204) se a atualização for bem-sucedida, 
        /// NotFound (HTTP 404) se a circular não for encontrada, 
        /// ou BadRequest (HTTP 400) se o id for inválido.
        /// </returns>
        /// <response code="204">Circular marcada como lida com sucesso.</response>
        /// <response code="404">Circular com o id especificado não foi encontrada.</response>
        /// <response code="400">O id fornecido é inválido.</response>
        [HttpPut("{id}/Lida")]
        public IActionResult PutCircularLida(int id)
        {
            return Requisicao.Manipulador(() =>
            {
                _circularesService.AtualizarComoLida(id);
                return NoContent();
            });
        }

        /// <summary>
        /// Deleta uma circular específica informando seu id.
        /// </summary>
        /// <param name="id">Id da Circular a ser deletada.</param>
        /// <returns>
        /// Retorna NoContent (HTTP 204) se a exclusão for bem-sucedida,
        /// NotFound (HTTP 404) se a circular não for encontrada,
        /// ou BadRequest (HTTP 400) se o id for inválido.
        /// </returns>
        /// <response code="204">Circular deletada com sucesso.</response>
        /// <response code="404">Circular com o id especificado não foi encontrada.</response>
        /// <response code="400">O id fornecido é inválido.</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteCircular([FromRoute] int id)
        {
            return Requisicao.Manipulador(() =>
            {
                _circularesService.DeletarCircular(id);

                return NoContent();
            });
        }
    }
}
