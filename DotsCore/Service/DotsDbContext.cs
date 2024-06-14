using DotsCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dots.Service
{
    public class DotsDbContext : DbContext
    {
        public DbSet<Score> Scores { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dots;Trusted_Connection=True;");
        }
    }
}