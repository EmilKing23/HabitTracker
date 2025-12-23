using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Data
{
    public class HabitRepository : IDisposable
    {
        private readonly HabitContext _context;
        public HabitRepository()
        {
            _context = new HabitContext();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                _context.Database.EnsureCreated();
                Console.WriteLine($"[i] База данных инициализирована: {_context.DbPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА] Не удалось инициализировать БД: {ex.Message}");
            }
        }

        public List<Habit> GetAllHabits()
        {
            return _context.Habits
                .OrderByDescending(h => h.CreatedDate)
                .ToList();
        }

        public Habit GetHabitById(int id)
        {
            return _context.Habits.Find(id);
        }

        public void AddHabit(Habit habit)
        {
            try
            {
                _context.Habits.Add(habit);
                _context.SaveChanges();
                Console.WriteLine($"[i] Привычка '{habit.Name}' сохранена в БД");
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Unique") == true)
            {
                Console.WriteLine($"[ОШИБКА] Привычка '{habit.Name}' уже существует!");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА] Не удалось сохранить привычку: {ex.Message}");
                throw;
            }
        }

        public void UpdateHabit(Habit habit)
        {
            _context.Habits.Update(habit);
            _context.SaveChanges();
        }

        public void DeleteHabit(int id)
        {
            var habit = _context.Habits.Find(id);
            if (habit != null)
            {
                _context.Habits.Remove(habit);
                _context.SaveChanges();
                Console.WriteLine($"[i] Привычка '{habit.Name}' удалена из БД");
            }
        }

        public List<Habit> SearchHabits(string searchTerm)
        {
            return _context.Habits
                .Where(h => EF.Functions.Like(h.Name, $"%{searchTerm}%"))
                .ToList();
        }

        public (int total, int completedToday, int createdToday) GetStatistics()
        {
            var today = DateTime.Today;

            int total = _context.Habits.Count();
            int completedToday = _context.Habits.Count(h => h.IsCompleted && h.LastCompletedDate.Value.Date == today);
            int createdToday = _context.Habits.Count(h => h.CreatedDate.Date == today);

            return (total, completedToday, createdToday);
        }

        public void ResetOldComplections()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var oldCompleted = _context.Habits
                .Where(h => h.IsCompleted && h.LastCompletedDate < yesterday)
                .ToList();

            foreach (var habit in oldCompleted)
            {
                _context.SaveChanges();
                Console.WriteLine($"[i] Сброшено {oldCompleted.Count} старых выполненных привычек");
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}