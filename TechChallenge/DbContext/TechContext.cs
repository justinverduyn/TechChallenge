using Microsoft.EntityFrameworkCore;
using TechChallenge.Models;

namespace TechChallenge
{
    class TechContext: DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }

        // The following configures EF to create a Sqlite database file as `C:\tech.db`.
        // For Mac or Linux, change this to `/tmp/tech.db` or any other absolute path.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C\tech.db");
    }
}