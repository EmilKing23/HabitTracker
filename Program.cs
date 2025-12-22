using System;
using System.Collections.Generic;
using System.Text.Json; // Для работы с JSON
using System.IO;        // Для работы с файлами

namespace HabitTracker
{
    class Program
    {
        // Статическое поле для хранения списка привычек
        private static List<Habit> habits = new List<Habit>();

        // Имя файла для сохранения данных
        private const string DataFileName = "habits.json";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Загружаем данные при запуске программы
            Console.WriteLine("[i] Загрузка данных...");
            LoadHabitsFromFile();
            System.Threading.Thread.Sleep(800);

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
                        SaveHabitsToFile();
                        Console.WriteLine("\nНажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case "8":
                        Console.WriteLine("\nСохранение данных...");
                        SaveHabitsToFile();
                        System.Threading.Thread.Sleep(1000);

                        exitRequested = true;
                        Console.WriteLine("До свидания! Возвращайтесь завтра!");
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

        static void ShowWelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine("          ТРЕКЕР ПРИВЫЧЕК v1.0");
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine();
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine($"СТАТИСТИКА: Всего привычек: {habits.Count}");
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine("1. [+] Добавить новую привычку");
            Console.WriteLine("2. [V] Показать все привычки");
            Console.WriteLine("3. [X] Отметить привычку выполненной");
            Console.WriteLine("4. [-] Удалить привычку");
            Console.WriteLine("5. [S] Найти привычку");
            Console.WriteLine("6. [%] Показать статистику");
            Console.WriteLine("7. [SAVE] Сохранить данные");
            Console.WriteLine("8. [>>] Выйти");
            Console.WriteLine("=".PadRight(50, '='));
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

            // Проверяем, нет ли уже такой привычки
            foreach (var habit in habits)
            {
                if (habit.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"\nПривычка '{name}' уже существует!");
                    return;
                }
            }

            // Создаем новый объект Habit
            Habit newHabit = new Habit(name);
            habits.Add(newHabit);

            // Автосохранение
            SaveHabitsToFile();

            Console.WriteLine($"\nПривычка '{name}' успешно добавлена!");
        }

        static void ShowAllHabits()
        {
            Console.Clear();
            Console.WriteLine("ВАШИ ПРИВЫЧКИ");
            Console.WriteLine("=".PadRight(50, '='));

            if (habits.Count == 0)
            {
                Console.WriteLine("Список привычек пуст. Добавьте первую привычку!");
                return;
            }

            for (int i = 0; i < habits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {habits[i]}");
            }
        }

        static void MarkHabitComplete()
        {
            ShowAllHabits();

            if (habits.Count == 0) return;

            Console.Write("\nВведите номер привычки для отметки: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= habits.Count)
            {
                habits[index - 1].MarkComplete();

                // Автосохранение
                SaveHabitsToFile();
            }
            else
            {
                Console.WriteLine("Неверный номер привычки!");
            }
        }

        static void DeleteHabit()
        {
            ShowAllHabits();

            if (habits.Count == 0) return;

            Console.Write("\nВведите номер привычки для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= habits.Count)
            {
                string habitName = habits[index - 1].Name;
                habits.RemoveAt(index - 1);

                // Автосохранение
                SaveHabitsToFile();

                Console.WriteLine($"Привычка '{habitName}' удалена!");
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

            if (habits.Count == 0)
            {
                Console.WriteLine("Список привычек пуст. Нечего искать.");
                Console.WriteLine("\nНажмите Enter для возврата в меню...");
                return;
            }

            Console.Write("Введите название или часть названия для поиска: ");
            string searchTerm = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("\nЗапрос не может быть пустым.");
                return;
            }

            // Поиск с помощью LINQ
            var foundHabits = habits
                .Where(habit => habit.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            Console.WriteLine($"\nРезультаты поиска по запросу \"{searchTerm}\":");
            Console.WriteLine("=".PadRight(50, '='));

            if (foundHabits.Count == 0)
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
            Console.WriteLine("СТАТИСТИКА ПРИВЫЧЕК");
            Console.WriteLine("=".PadRight(50, '='));

            if (habits.Count == 0)
            {
                Console.WriteLine("Список привычек пуст. Нет данных для статистики.");
                Console.WriteLine("\nНажмите Enter для возврата в меню...");
                return;
            }

            // 1. Основные счетчики
            int totalHabits = habits.Count;
            int completedToday = habits.Count(h => h.IsCompleted);
            double completionRate = totalHabits > 0 ? (double)completedToday / totalHabits * 100 : 0;

            // 2. Работа с датами
            DateTime today = DateTime.Today;
            int createdToday = habits.Count(h => h.CreatedDate.Date == today);

            // 3. Поиск рекордов
            int longestStreak = habits.Max(h => h.Streak);
            var habitWithLongestStreak = habits.FirstOrDefault(h => h.Streak == longestStreak);
            var oldestHabit = habits.OrderBy(h => h.CreatedDate).FirstOrDefault();
            var newestHabit = habits.OrderByDescending(h => h.CreatedDate).FirstOrDefault();

            // 4. Вывод статистики
            Console.WriteLine($"Всего привычек: {totalHabits}");
            Console.WriteLine($"Создано сегодня: {createdToday}");
            Console.WriteLine($"Выполнено сегодня: {completedToday} из {totalHabits} ({completionRate:F1}%)");
            Console.WriteLine($"Самая длинная серия: {longestStreak} дн. " +
                             (habitWithLongestStreak != null ? $"({habitWithLongestStreak.Name})" : ""));
            Console.WriteLine();

            if (oldestHabit != null)
            {
                Console.WriteLine($"Старейшая привычка: {oldestHabit.Name} " +
                                 $"(с {oldestHabit.CreatedDate:dd.MM.yyyy})");
            }

            if (newestHabit != null)
            {
                Console.WriteLine($"Последняя добавленная: {newestHabit.Name} " +
                                 $"(с {newestHabit.CreatedDate:dd.MM.yyyy})");
            }

            // 5. Прогресс-бар
            Console.WriteLine("\nПрогресс выполнения сегодня:");
            DrawProgressBar(completedToday, totalHabits);
        }

        // Вспомогательный метод для рисования прогресс-бара
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

        // Метод для сохранения привычек в файл JSON
        static void SaveHabitsToFile()
        {
            try
            {
                // Настройки сериализации
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // Преобразуем список в JSON-строку
                string jsonString = JsonSerializer.Serialize(habits, options);

                // Записываем JSON в файл
                File.WriteAllText(DataFileName, jsonString);

                Console.WriteLine($"\n[i] Данные сохранены в файл: {DataFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ОШИБКА] Не удалось сохранить данные: {ex.Message}");
            }
        }

        // Метод для загрузки привычек из файла JSON
        static void LoadHabitsFromFile()
        {
            try
            {
                // Проверяем, существует ли файл
                if (!File.Exists(DataFileName))
                {
                    Console.WriteLine("[i] Файл данных не найден. Будет создан новый.");
                    return;
                }

                // Читаем JSON из файла
                string jsonString = File.ReadAllText(DataFileName);

                // Преобразуем JSON-строку обратно в список привычек
                var loadedHabits = JsonSerializer.Deserialize<List<Habit>>(jsonString);

                if (loadedHabits != null)
                {
                    habits = loadedHabits;
                    Console.WriteLine($"[i] Загружено {habits.Count} привычек из файла.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ОШИБКА] Не удалось загрузить данные: {ex.Message}");
                Console.WriteLine("[i] Будет создан новый список привычек.");
            }
        }
    }
}