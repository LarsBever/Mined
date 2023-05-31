using Microsoft.EntityFrameworkCore;
using Mined.Domain;
using MySql.EntityFrameworkCore.Extensions;

namespace Mined.Data
{
    public class MinedDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=MinedDB;user=root;password=BuongiornoMesAmigos4321&!")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information );
                // .EnableSensitiveDataLogging()

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Payload> Payloads { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Uxo> Uxos { get; set; }
        public DbSet<UxoPayload> UxoPayloads { get; set; }
  }
}