using Mango.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //use for identity services identity
            base.OnModelCreating(modelBuilder);

        }
    }
}
