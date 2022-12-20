using Alten.Booking.Application.Abstractions;
using Alten.Booking.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Alten.Booking.Infrastructure.Persistence
{
    public class ApplicationContext : DbContext, IUnitOfWork
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Guest> Guests { get; set; }

        public string DbPath { get; }

        public ApplicationContext(ILogger<ApplicationContext> logger)
        {
            Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
            string? path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "booking.db");
            logger.LogInformation($"Database file path: {DbPath}");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}", options =>
            {
                options.MigrationsAssembly("Alten.Booking.Infrastructure");
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
