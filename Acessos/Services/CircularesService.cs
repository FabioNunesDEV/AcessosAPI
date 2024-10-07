using Acessos.Data;
using Acessos.DTO.Circular;
using Acessos.Models;
using AutoMapper;

namespace Acessos.Services;

public class CircularesService
{
    private readonly AcessoApiContext _context;
    private readonly IMapper _mapper;

    public CircularesService(AcessoApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Circular CadastrarCircular(CircularCreateDTO dto)
    {
        var circular = _mapper.Map<Circular>(dto);

        // Preenche campos automaticamente.
        circular.Protocolo = Guid.NewGuid().ToString();
        circular.DataEnvio = DateTime.Now;
        circular.Status = "Pendente";

        _context.Circulares.Add(circular);
        _context.SaveChanges();

        return circular;
    }

    public List<CircularReadDTO> ObterListaCirculares(int skip, int take) 
    {
        var circulares = _mapper.Map<List<CircularReadDTO>>(_context.Circulares.Skip(skip).Take(take));
        return circulares;
    }

    public CircularReadDTO ObterCircularPorId(int id)
    {
        this.ValidarId(id);

        var circular = this.ObterCircularCadastrada(id);

        return _mapper.Map<CircularReadDTO>(circular);
    }

    public void AtualizarCircular(int id, CircularUpdateDTO dto)
    {
        this.ValidarId(id);

        var circular = this.ObterCircularCadastrada(id);

        _mapper.Map(dto, circular);
        _context.SaveChanges();
    }

    public void AtualizarComoLida(int id)
    {

        this.ValidarId(id);

        var circular = this.ObterCircularCadastrada(id);

        circular.DataRecebimento = DateTime.Now;
        circular.Status = "Lida";

        _context.SaveChanges();
    }

    public void DeletarCircular(int id)
    {
        this.ValidarId(id);

        var circular = this.ObterCircularCadastrada(id);

        _context.Circulares.Remove(circular);
        _context.SaveChanges();
    }

    private void ValidarId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("O Id deve ser um número maior que zero.");
        }
    }

    private  Circular ObterCircularCadastrada(int id)
    {
        var circular = _context.Circulares.FirstOrDefault(c => c.Id == id);
        if (circular == null)
        {
            throw new KeyNotFoundException($"Circular com Id {id} não encontrada.");
        }

        return circular;
    }
}
