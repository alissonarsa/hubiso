using hubiso.Models;
using Microsoft.EntityFrameworkCore;

namespace hubiso.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Esta linha diz ao Entity Framework para gerir uma tabela de Materiais
        public DbSet<Material> Materiais { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
    }
}