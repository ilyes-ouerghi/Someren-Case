using Microsoft.EntityFrameworkCore;
using Someren_Case.Models;
using System.Collections.Generic;


namespace Someren_Case.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Student> Student { get; set; }
    }
}