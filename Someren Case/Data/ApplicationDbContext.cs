using Microsoft.EntityFrameworkCore;
using Someren_Case.Models; // Adjust this based on your models namespace

namespace Someren_Case.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Room> Room { get; set; }
    }
}