using Acessos.Data;
using Acessos.DTO.Circular;
using Acessos.Models;
using AutoMapper;

namespace Acessos.Services
{
    public class CircularesService
    {
        private readonly AcessoApiContext _context;
        private readonly IMapper _mapper;

        public CircularesService (AcessoApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CadastrarCircular(CircularCreateDTO dto)
        {
            var circular = _mapper.Map<Circular>(dto);

            // Preenche campos automaticamente.
            circular.Protocolo = Guid.NewGuid().ToString();
            circular.DataEnvio = DateTime.Now;
            circular.Status = "Pendente";

            _context.Circulares.Add(circular);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCircularPorId), new { id = circular.Id }, circular);
        }
    }
}
