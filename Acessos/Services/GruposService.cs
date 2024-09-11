using Acessos.Data;
using Acessos.DTO.Grupo;
using Acessos.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Acessos.Services
{
    public class GruposService
    {
        private readonly AcessoApiContext _context;
        private readonly IMapper _mapper;

        public GruposService(AcessoApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Grupo CadastrarGrupo(GrupoCreateDTO dto)
        {
            var grupo = _mapper.Map<Grupo>(dto);
            _context.Grupos.Add(grupo);
            _context.SaveChanges();
            return grupo;
        }

        public List<GrupoReadDTO> ObterListaGrupos(int skip, int take)
        {
            var grupos = _mapper.Map<List<GrupoReadDTO>>(_context.Grupos.Skip(skip).Take(take));
            return grupos;
        }

        public GrupoReadDTO ObterGrupoPorId(int id)
        {
            ValidarId(id);
            var grupo = ObterGrupoCadastrado(id);
            return _mapper.Map<GrupoReadDTO>(grupo);
        }

        public Object ObterGrupoPorIdUsuarios(int id)
        {
            this.ValidarId(id);

            var grupo = _context.Grupos
                .Include(g => g.UsuarioGrupos)
                    .ThenInclude(ug => ug.Usuario)
                .FirstOrDefault(g => g.Id == id);

            if (grupo == null) throw new KeyNotFoundException($"Grupo com Id {id} não encontrado.");

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

            return response;
        }

        public void AtualizarGrupo(int id, GrupoUpdateDTO dto)
        {
            ValidarId(id);
            var grupo = ObterGrupoCadastrado(id);
            _mapper.Map(dto, grupo);
            _context.SaveChanges();
        }

        public void AtualizarGrupoParcialmente(int id, JsonPatchDocument<GrupoUpdateDTO> patchDoc)
        {
            ValidarId(id);
            var grupo = ObterGrupoCadastrado(id);
            var grupoDTO = _mapper.Map<GrupoUpdateDTO>(grupo);
            patchDoc.ApplyTo(grupoDTO);
            _mapper.Map(grupoDTO, grupo);
            _context.SaveChanges();
        }

        public void DeletarGrupo(int id)
        {
            ValidarId(id);
            var grupo = ObterGrupoCadastrado(id);

            // Verifica se o grupo tem usuários associados
            bool hasUsers = _context.UsuarioGrupos.Any(ug => ug.GrupoId == id);
            if (hasUsers)
            {
                throw new InvalidOperationException("Não é possível deletar o grupo porque ele possui usuários associados. Exclua as relações primeiro.");
            }

            _context.Grupos.Remove(grupo);
            _context.SaveChanges();
        }

        private void ValidarId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O Id deve ser um número maior que zero.");
            }
        }

        private Grupo ObterGrupoCadastrado(int id)
        {
            var grupo = _context.Grupos.FirstOrDefault(g => g.Id == id);
            if (grupo == null)
            {
                throw new KeyNotFoundException($"Grupo com Id {id} não encontrado.");
            }
            return grupo;
        }
    }
}
