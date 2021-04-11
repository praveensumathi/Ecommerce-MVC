using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, AppRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) :base(context)
        {
        }

        public virtual DbSet<Product> Product { get { return Set<Product>(); } }
        public virtual DbSet<ApplicationUser> Users { get; set; }
    }
}
