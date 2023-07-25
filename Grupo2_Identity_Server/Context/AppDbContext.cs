using Grupo2_Identity_Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Grupo2_Identity_Server.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
