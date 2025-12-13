using Microsoft.EntityFrameworkCore;

namespace API_de_Gestión_de_Usuarios.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
        }

        
    }
}
