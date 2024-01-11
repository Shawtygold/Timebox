using Microsoft.EntityFrameworkCore;

namespace Timebox.MVVM.Model
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Alarm> Alarms => Set<Alarm>();
        public DbSet<Hourglass> Hourglasses => Set<Hourglass>();
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=timeboxapp.db");
        }
    }
}
