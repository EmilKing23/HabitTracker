using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Data
{
    public class HabitContext : DbContext
    {
        public DbSet<Habit> Habits { get; set; }

        public string DbPath { get;}

        public HabitContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "habitTracker.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Habit>()
                .HasIndex(h => h.Name)
                .IsUnique();

            modelBuilder.Entity<Habit>()
                .Property(h => h.Name)
                .IsRequired();

            modelBuilder.Entity<Habit>()
                .Property(h => h.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
