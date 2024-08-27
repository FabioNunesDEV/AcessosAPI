using Acessos.Data;
using Acessos.DTO.usuario;
using Acessos.Models;
using Acessos.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Acessos.Services
{
    public class UsuariosService
    {
        private readonly AcessoApiContext _context;
        private readonly IMapper _mapper;

        public UsuariosService(AcessoApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Usuario CadastrarUsuario(UsuarioCreateDTO dto)
        {
            var usuario = _mapper.Map<Usuario>(dto);

            usuario.Salt = Util.GerarSalt();
            usuario.Senha = Util.GerarHash(usuario.Senha + "-" + usuario.Salt);

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }

        public UsuarioReadDTO ObterUsuarioPorId(int id)
        {
            this.ValidarId(id);

            var usuario = this.ObterUsuarioCadastrado(id);

            return _mapper.Map<UsuarioReadDTO>(usuario);
        }

        public Object ObterUsuarioPorIdGrupos(int id)
        {

            ValidarId(id);
            var usuario = _context.Usuarios
                .Include(u => u.UsuarioGrupos)
                    .ThenInclude(ug => ug.Grupo)
                .FirstOrDefault(u => u.Id == id);

            if (usuario == null) throw new KeyNotFoundException($"Usuário com Id {id} não encontrado.");

            var response = new
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Grupos = usuario.UsuarioGrupos.Select(ug => new
                {
                    Id = ug.Grupo.Id,
                    Nome = ug.Grupo.Nome
                }).ToList()
            };

            return response;
        }

        public List<UsuarioReadDTO> ObterListaUsuarios(int skip, int take)
        {
            var usuarios = _mapper.Map<List<UsuarioReadDTO>>(_context.Usuarios.Skip(skip).Take(take));
            return usuarios;
        }

        public void AtualizarUsuario(int id, UsuarioUpdateDTO dto)
        {
            ValidarId(id);
            var usuario = ObterUsuarioCadastrado(id);
            _mapper.Map(dto, usuario);
            _context.SaveChanges();
        }

        public void AtualizarUsuarioParcialmente(int id, JsonPatchDocument<UsuarioUpdateDTO> patchDoc)
        {
            ValidarId(id);
            var usuario = ObterUsuarioCadastrado(id);
            var usuarioDTO = _mapper.Map<UsuarioUpdateDTO>(usuario);
            patchDoc.ApplyTo(usuarioDTO);
            _mapper.Map(usuarioDTO, usuario);
            _context.SaveChanges();
        }

        public void DeletarUsuario(int id)
        {
            ValidarId(id);
            var usuario = ObterUsuarioCadastrado(id);
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        public void AdicionarUsuarioGrupo(int usuarioId, int grupoId)
        {
            ValidarId(usuarioId);
            ValidarId(grupoId);

            var usuario = ObterUsuarioCadastrado(usuarioId);
            var grupo = _context.Grupos.FirstOrDefault(g => g.Id == grupoId);

            if (grupo == null) throw new KeyNotFoundException($"Grupo com Id {grupoId} não encontrado.");

            var usuarioGrupoExistente = _context.UsuarioGrupos.FirstOrDefault(ug => ug.UsuarioId == usuarioId && ug.GrupoId == grupoId);
            if (usuarioGrupoExistente != null) throw new ArgumentException("Este usuário já está associado a este grupo.");

            var usuarioGrupo = new UsuarioGrupo
            {
                UsuarioId = usuarioId,
                GrupoId = grupoId
            };

            _context.UsuarioGrupos.Add(usuarioGrupo);
            _context.SaveChanges();
        }

        public void RemoverUsuarioGrupo(int usuarioId, int grupoId)
        {
            ValidarId(usuarioId);
            ValidarId(grupoId);

            var usuario = ObterUsuarioCadastrado(usuarioId);
            var grupo = _context.Grupos.FirstOrDefault(g => g.Id == grupoId);

            if (grupo == null) throw new KeyNotFoundException($"Grupo com Id {grupoId} não encontrado.");

            var usuarioGrupoExistente = _context.UsuarioGrupos.FirstOrDefault(ug => ug.UsuarioId == usuarioId && ug.GrupoId == grupoId);
            if (usuarioGrupoExistente == null) throw new ArgumentException("Este usuário não está associado a este grupo.");

            _context.UsuarioGrupos.Remove(usuarioGrupoExistente);
            _context.SaveChanges();
        }

        private void ValidarId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O Id deve ser um número maior que zero.");
            }
        }

        private Usuario ObterUsuarioCadastrado(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                throw new KeyNotFoundException($"Usuário com Id {id} não encontrado.");
            }
            return usuario;
        }
    }
}
