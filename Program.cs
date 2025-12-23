using System;
using System.Collections.Generic;
using System.Linq;
using HabitTracker.Data;

namespace HabitTracker
{
    class Program
    {
        private static HabitRepository _repository;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            using (_repository = new HabitRepository())
            {
                // Сбрасываем старые выполнения
                _repository.ResetOldComplections();

                ShowWelcomeMessage();

                bool exitRequested = false;

                while (!exitRequested)
                {
                    ShowMenu();
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddNewHabit();
                            break;
                        case "2":
                            ShowAllHabits();
                            break;
                        case "3":
                            MarkHabitComplete();
                            break;
                        case "4":
                            DeleteHabit();
                            break;
                        case "5":
                            SearchHabit();
                            break;
                        case "6":
                            ShowStatistics();
                            break;
                        case "7":
                            ShowAdvancedStatistics();
                            break;
                        case "8":
                            exitRequested = true;
                            Console.WriteLine("\nДо свидания! Данные сохранены в базе данных.");
                            break;
                        default:
                            Console.WriteLine("\nНеверный выбор. Попробуйте снова.");
                            System.Threading.Thread.Sleep(1000);
                            break;
                    }

                    if (!exitRequested)
                    {
                        Console.WriteLine("\nНажмите Enter для продолжения...");
                        Console.ReadLine();
                    }
                }
            }
        }

        static void ShowWelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("       ТРЕКЕР ПРИВЫЧЕК v2.0 (SQLite + EF Core)");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();
        }

        static void ShowMenu()
        {
            var stats = _repository.GetStatistics();

            Console.Clear();
            Console.WriteLine($"СТАТИСТИКА: Всего: {stats.total} | Выполнено сегодня: {stats.completedToday} | Новых сегодня: {stats.createdToday}");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("1. [+] Добавить новую привычку");
            Console.WriteLine("2. [V] Показать все привычки");
            Console.WriteLine("3. [X] Отметить привычку выполненной");
            Console.WriteLine("4. [-] Удалить привычку");
            Console.WriteLine("5. [S] Найти привычку");
            Console.WriteLine("6. [%] Основная статистика");
            Console.WriteLine("7. [%%] Расширенная статистика");
            Console.WriteLine("8. [>>] Выйти");
            Console.WriteLine("=".PadRight(60, '='));
            Console.Write("Выберите действие (1-8): ");
        }

        static void AddNewHabit()
        {
            Console.Clear();
            Console.WriteLine("ДОБАВЛЕНИЕ НОВОЙ ПРИВЫЧКИ");
            Console.WriteLine("=".PadRight(50, '='));

            Console.Write("Введите название привычки: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("\nНазвание не может быть пустым!");
                return;
            }

            if (name.Length > 100)
            {
                Console.WriteLine("\nНазвание слишком длинное (макс. 100 символов)!");
                return;
            }

            try
            {
                var habit = new Habit(name);
                _repository.AddHabit(habit);
                Console.WriteLine($"\nПривычка '{name}' успешно добавлена в базу данных!");
            }
            catch
            {
                Console.WriteLine("\nНе удалось добавить привычку.");
            }
        }

        static void ShowAllHabits()
        {
            Console.Clear();
            Console.WriteLine("ВАШИ ПРИВЫЧКИ");
            Console.WriteLine("=".PadRight(50, '='));

            var habits = _repository.GetAllHabits();

            if (!habits.Any())
            {
                Console.WriteLine("Список привычек пуст. Добавьте первую привычку!");
                return;
            }

            Console.WriteLine($"Всего привычек: {habits.Count}\n");

            for (int i = 0; i < habits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {habits[i]}");
            }
        }

        static void MarkHabitComplete()
        {
            var habits = _repository.GetAllHabits();

            if (!habits.Any())
            {
                Console.WriteLine("Список привычек пуст.");
                return;
            }

            Console.Clear();
            Console.WriteLine("ОТМЕТКА ВЫПОЛНЕНИЯ ПРИВЫЧКИ");
            Console.WriteLine("=".PadRight(50, '='));

            // Показываем только не выполненные сегодня привычки
            var notCompletedToday = habits
                .Where(h => !h.IsCompleted || h.LastCompletedDate?.Date != DateTime.Today)
                .ToList();

            if (!notCompletedToday.Any())
            {
                Console.WriteLine("Все привычки уже выполнены сегодня!");
                return;
            }

            for (int i = 0; i < notCompletedToday.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {notCompletedToday[i].Name}");
            }

            Console.Write("\nВведите номер привычки для отметки: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= notCompletedToday.Count)
            {
                var habit = notCompletedToday[index - 1];
                habit.MarkComplete();
                _repository.UpdateHabit(habit);
            }
            else
            {
                Console.WriteLine("Неверный номер привычки!");
            }
        }

        static void DeleteHabit()
        {
            var habits = _repository.GetAllHabits();

            if (!habits.Any())
            {
                Console.WriteLine("Список привычек пуст.");
                return;
            }

            Console.Clear();
            Console.WriteLine("УДАЛЕНИЕ ПРИВЫЧКИ");
            Console.WriteLine("=".PadRight(50, '='));

            for (int i = 0; i < habits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {habits[i].Name}");
            }

            Console.Write("\nВведите номер привычки для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= habits.Count)
            {
                var habit = habits[index - 1];
                _repository.DeleteHabit(habit.Id);
                Console.WriteLine($"\nПривычка '{habit.Name}' удалена!");
            }
            else
            {
                Console.WriteLine("Неверный номер привычки!");
            }
        }

        static void SearchHabit()
        {
            Console.Clear();
            Console.WriteLine("ПОИСК ПРИВЫЧКИ");
            Console.WriteLine("=".PadRight(50, '='));

            Console.Write("Введите название или часть названия для поиска: ");
            string searchTerm = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("\nЗапрос не может быть пустым.");
                return;
            }

            var foundHabits = _repository.SearchHabits(searchTerm);

            Console.WriteLine($"\nРезультаты поиска по запросу \"{searchTerm}\":");
            Console.WriteLine("=".PadRight(50, '='));

            if (!foundHabits.Any())
            {
                Console.WriteLine("Привычки не найдены.");
            }
            else
            {
                Console.WriteLine($"Найдено привычек: {foundHabits.Count}\n");
                for (int i = 0; i < foundHabits.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {foundHabits[i]}");
                }
            }
        }

        static void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("ОСНОВНАЯ СТАТИСТИКА");
            Console.WriteLine("=".PadRight(50, '='));

            var habits = _repository.GetAllHabits();
            var stats = _repository.GetStatistics();

            if (!habits.Any())
            {
                Console.WriteLine("Список привычек пуст. Нет данных для статистики.");
                return;
            }

            // Основные метрики
            double completionRate = stats.total > 0 ? (double)stats.completedToday / stats.total * 100 : 0;

            // Дополнительные метрики из БД
            var longestStreak = habits.Max(h => h.Streak);
            var bestHabit = habits.FirstOrDefault(h => h.Streak == longestStreak);
            var averageStreak = habits.Average(h => h.Streak);

            Console.WriteLine($"Всего привычек: {stats.total}");
            Console.WriteLine($"Создано сегодня: {stats.createdToday}");
            Console.WriteLine($"Выполнено сегодня: {stats.completedToday} из {stats.total} ({completionRate:F1}%)");
            Console.WriteLine($"Самая длинная серия: {longestStreak} дн. " +
                             (bestHabit != null ? $"({bestHabit.Name})" : ""));
            Console.WriteLine($"Средняя серия: {averageStreak:F1} дн.");

            // Прогресс-бар
            Console.WriteLine("\nПрогресс выполнения сегодня:");
            DrawProgressBar(stats.completedToday, stats.total);
        }

        static void ShowAdvancedStatistics()
        {
            Console.Clear();
            Console.WriteLine("РАСШИРЕННАЯ СТАТИСТИКА");
            Console.WriteLine("=".PadRight(50, '='));

            var habits = _repository.GetAllHabits();

            if (!habits.Any())
            {
                Console.WriteLine("Список привычек пуст.");
                return;
            }

            // Анализ по датам
            var today = DateTime.Today;
            var weekAgo = today.AddDays(-7);

            var recentHabits = habits
                .Where(h => h.CreatedDate >= weekAgo)
                .GroupBy(h => h.CreatedDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToList();

            Console.WriteLine("Новые привычки за последние 7 дней:");
            foreach (var day in recentHabits)
            {
                Console.WriteLine($"  {day.Date:dd.MM}: {day.Count} привычек");
            }

            // Статистика выполнения
            var completionStats = habits
                .GroupBy(h => h.Streak)
                .Select(g => new { Streak = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            Console.WriteLine("\nРаспределение по длине серий:");
            foreach (var stat in completionStats.Take(5))
            {
                Console.WriteLine($"  Серия {stat.Streak} дней: {stat.Count} привычек");
            }

            // Самые старые привычки
            var oldestHabits = habits
                .OrderBy(h => h.CreatedDate)
                .Take(3)
                .ToList();

            Console.WriteLine("\nСамые старые привычки:");
            foreach (var habit in oldestHabits)
            {
                var daysOld = (today - habit.CreatedDate.Date).Days;
                Console.WriteLine($"  {habit.Name} ({daysOld} дней)");
            }
        }

        static void DrawProgressBar(int completed, int total)
        {
            if (total <= 0) return;

            const int barWidth = 30;
            int filledWidth = (int)Math.Round((double)completed / total * barWidth);

            Console.Write("[");
            Console.Write(new string('#', filledWidth));
            Console.Write(new string('-', barWidth - filledWidth));
            Console.WriteLine($"] {completed}/{total}");
        }
    }
}