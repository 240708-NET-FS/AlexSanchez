using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HabitTracker.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}

    public ApplicationDbContext(){}


    public DbSet<Habit> Habits {get;set;}
    public DbSet<DailyEntry> DailyEntries {get;set;}


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json")
                                            .Build();
            
            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DailyEntry>()
                .HasOne(de => de.Habit)
                .WithMany(h => h.DailyEntries)
                .HasForeignKey(de => de.HabitID);
    }
}