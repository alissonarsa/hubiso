using hubiso.Models;
using Microsoft.EntityFrameworkCore;

namespace hubiso.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Material> Materiais { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Solicitante> Solicitantes { get; set; }
    }
}