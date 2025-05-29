using Microsoft.EntityFrameworkCore;
using NotoriumAPI.Models;

namespace NotoriumAPI
{
    public class NotoriumDbContext : DbContext
    {
        public NotoriumDbContext(DbContextOptions<NotoriumDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SheetMusic> SheetMusic { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasMany(u => u.SheetMusic)
                .WithOne(sm => sm.User)
                .HasForeignKey(sm => sm.UserId);

            modelBuilder.Entity<SheetMusic>()
                .Property(sm => sm.Genre)
                .HasConversion<string>();
            modelBuilder.Entity<SheetMusic>()
                .Property(sm => sm.Instrument)
                .HasConversion<string>();
            modelBuilder.Entity<SheetMusic>()
                .Property(sm => sm.Difficulty)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteSheetMusic)
                .WithMany(sm => sm.FavoritedByUsers)
                .UsingEntity(j => j.ToTable("UserFavoriteSheetMusic"));

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<SheetMusic>().ToTable("SheetMusic");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
