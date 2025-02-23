using Microsoft.EntityFrameworkCore;
using SignUpAuthentication.Model;

namespace SignUpAuthentication.Data
{
    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {

        }
        public DbSet<UserDb> Users { get; set; }
        public DbSet<UserRegistrationDb> Registration { get; set;}

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelBuilder.Entity<UserDb>()
                .HasIndex(u => u.Id)
                .IsUnique();
    }
*/
    }

}
