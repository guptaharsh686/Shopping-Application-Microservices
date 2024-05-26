using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //use for identity services identity
            base.OnModelCreating(modelBuilder);

        }
    }
}
