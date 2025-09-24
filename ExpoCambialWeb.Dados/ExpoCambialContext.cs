using Microsoft.EntityFrameworkCore;
using ExpoCambialWeb.Entidades;

namespace ExpoCambialWeb.Dados
{
    public class ExpoCambialContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<ExpoCambialRegistro> ExpoCambialRegistros { get; set; }

        public DbSet<UsuarioAuth> UsuarioAuth { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseSqlServer("Server=localhost;Database=TESTE;Trusted_Connection=True;TrustServerCertificate=True;");
#else
            optionsBuilder.UseSqlServer("Server=localhost;Database=BD_AGENDA_CONSULTA;Trusted_Connection=True;TrustServerCertificate=True;");
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações específicas se necessário
            modelBuilder.Entity<UsuarioAuth>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}