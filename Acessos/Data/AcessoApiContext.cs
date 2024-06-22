using Acessos.Models;
using Microsoft.EntityFrameworkCore;

namespace Acessos.Data;

public class AcessoApiContext : DbContext
{
    public AcessoApiContext(DbContextOptions<AcessoApiContext> opts): base(opts)
    {

    }

    public DbSet<Usuario> Usuarios { get; set; }

}
