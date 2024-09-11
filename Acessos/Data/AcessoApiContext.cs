using Acessos.Models;
using Microsoft.EntityFrameworkCore;

namespace Acessos.Data;

public class AcessoApiContext : DbContext
{
    public AcessoApiContext(DbContextOptions<AcessoApiContext> opts): base(opts)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cria chave composta para modelo UsuarioGrupo
        modelBuilder.Entity<UsuarioGrupo>()
            .HasKey(UsuarioGrupo => new { UsuarioGrupo.UsuarioId, UsuarioGrupo.GrupoId });

        // Cria relacionamento UsuarioGrupo -> Usuario
        modelBuilder.Entity<UsuarioGrupo>()
            .HasOne(ug => ug.Usuario)
            .WithMany(u => u.UsuarioGrupos)
            .HasForeignKey(ug => ug.UsuarioId);

        // Cria relacionamento UsuarioGrupo -> Grupo
        modelBuilder.Entity<UsuarioGrupo>()
            .HasOne(ug => ug.Grupo)
            .WithMany(g => g.UsuarioGrupos)
            .HasForeignKey(ug => ug.GrupoId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Circular> Circulares { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<UsuarioGrupo> UsuarioGrupos { get; set; }

}
