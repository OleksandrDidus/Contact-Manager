using Contact_Manager.Models;
using Microsoft.EntityFrameworkCore;
namespace Contact_Manager
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }

}
