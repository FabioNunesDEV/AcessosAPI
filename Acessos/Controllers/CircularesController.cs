using Acessos.Models;
using Acessos.DTO.Circular;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Acessos.Data;
using System.Net;

namespace Acessos.Controllers
{
    [ApiController]
    [Route("api/v1/circulares")]
    public class CircularesController : ControllerBase
    {
        private readonly AcessoApiContext _context;
        private readonly IMapper _mapper;

        public CircularesController(AcessoApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria um novo registro para Circular
        /// </summary>
        /// <param name="circularDTO">Objeto DTO com informações da circular</param>
        [HttpPost]
        [ProducesResponseType(typeof(Circular), (int)HttpStatusCode.Created)]
        public IActionResult PostCircular([FromBody] CircularCreateDTO circularDTO)
        {
            var circular = _mapper.Map<Circular>(circularDTO);

            // Preenche campos automaticamente.
            circular.Protocolo = Guid.NewGuid().ToString();
            circular.DataEnvio = DateTime.Now;
            circular.Status = "Pendente";

            _context.Circulares.Add(circular);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCircularPorId), new { id = circular.Id }, circular);
        }

        /// <summary>
        /// Retorna lista de circulares paginada.
        /// </summary>
        /// <param name="skip">Posição inicial</param>
        /// <param name="take">Quantos regstros serão obtidos a partir da posição inicial</param>
        /// <returns>Retorna lista de Cirulares.</returns>
        [HttpGet]
        public IEnumerable<CircularReadDTO> GetCircularLista([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var circulares = _mapper.Map<List<CircularReadDTO>>(_context.Circulares.Skip(skip).Take(take));
            return circulares;
        }

        /// <summary>
        /// Recupera infromações de uma circular informando o Id
        /// </summary>
        /// <param name="id">Id da circular</param>
        [HttpGet("{id}")]
        public IActionResult GetCircularPorId([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("O Id deve ser um número maior que zero.");
            }

            var circular = _context.Circulares.FirstOrDefault(c => c.Id == id);
            if (circular == null)
            {
                return NotFound($"Circular com Id {id} não encontrada.");
            }

            var circularDTO = _mapper.Map<CircularReadDTO>(circular);
            return Ok(circularDTO);
        }

        /// <summary>
        /// Atualiza registro de Circular.
        /// </summary>
        /// <param name="id">Id da circular</param>
        /// <param name="circularDTO">Objeto DTO da circular</param>
        [HttpPut("{id}")]
        public IActionResult PutCircular(int id, [FromBody] CircularUpdateDTO circularDTO)
        {
            if (id <= 0)
            {
                return BadRequest("O Id deve ser um número maior que zero");
            }

            var circular = _context.Circulares.FirstOrDefault(c => c.Id == id);
            if (circular == null)
            {
                return NotFound($"Circular com Id {id} não encontrada.");
            }

            _mapper.Map(circularDTO, circular);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/Lida")]
        public IActionResult PutCircularLida(int id)
        {
            if (id <= 0)
            {
                return BadRequest("O Id deve ser um número maior que zero");
            }

            var circular = _context.Circulares.FirstOrDefault(c => c.Id == id);
            if (circular == null)
            {
                return NotFound($"Circular com Id {id} não encontrada.");
            }

            circular.DataRecebimento=DateTime.Now;
            circular.Status = "Lida";

            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deleta uma circular específica informando seu id.
        /// </summary>
        /// <param name="id">Id da Circular</param>
        [HttpDelete("{id}")]
        public IActionResult DeleteCircular([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("O Id deve ser um número maior que zero");
            }

            var circular = _context.Circulares.FirstOrDefault(c => c.Id == id);
            if (circular == null)
            {
                return NotFound($"Circular com Id {id} não encontrada.");
            }

            _context.Circulares.Remove(circular);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
