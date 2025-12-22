using System;
using System.Collections.Generic;

namespace HabitTracker
{
    class Program
    {
        private static List<Habit> habits = new List<Habit>();
        static void Main(string[] args)
        {

            ShowWelcomeMessage();

            bool exitRequested = false;

            while(!exitRequested)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewHabit();
                        break;
                    case "2":
                        ShowAllHabbits();
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
                        exitRequested = true;
                        Console.WriteLine("\n До свидания! Возвращайтесь завтра!");
                        break;
                    default:
                        Console.WriteLine("\n Неверный выбор. Попробуйте снова.");
                        break;
                }

                if (!exitRequested)
                {
                    Console.WriteLine("\nНажмите Enter для продолжения...");
                    Console.ReadLine();
                }
            }
        }

        static void SearchHabit()
        {
            Console.Clear();
            Console.WriteLine("ПОИСК ПРИВЫЧКИ");
            Console.WriteLine("=".PadRight(50, '='));

            if(habits.Count == 0)
            {
                Console.WriteLine("Список привычек пуст. Нечего искать.");
                Console.WriteLine("\nНажмите Enter для возврата в меню...");
                return;
            }

            Console.Write("Введите название или часть названия для поиска: ");
            string SearchTerm = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Console.WriteLine("\nЗапрос не может быть пустым.");
                return;
            }

            var foundHabits = new List<Habit>();
            foreach (var habit in habits)
            {
                if (habit.Name.Contains(SearchTerm, StringComparison.CurrentCultureIgnoreCase))
                {
                    foundHabits.Add(habit);
                }
            }

            Console.WriteLine($"\nРезультаты поиска по запросу \"{SearchTerm}\":");
            Console.WriteLine("=".PadRight(50, '='));

            if (foundHabits.Count == 0)
            {
                Console.WriteLine("Ппривычки не найдены");
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

        

        static void ShowWelcomeMessage() 
        {
            Console.Clear();
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine("           ТРЕКЕР ПРИВЫЧЕК v1.0");
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine();
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine($"📊 Статистика: Всего привычек: {habits.Count}");
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine("1. Добавить новую привычку");
            Console.WriteLine("2. Показать все привычки");
            Console.WriteLine("3. Отметить привычку выполненной");
            Console.WriteLine("4. Удалить привычку");
            Console.WriteLine("5. Найти привычку");
            Console.WriteLine("6. Выйти");
            Console.WriteLine("=".PadRight(50, '='));
            Console.Write("Выберите действие (1-5): ");
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

            foreach (var habit in habits)
            {
                if (habit.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"\nПривычка '{name}' уже существует!");
                    return;
                }
            }
         
            Habit newHabit = new Habit(name);
            habits.Add(newHabit);

            Console.WriteLine($"\nПривычка '{name}' успешно добавлена!");
        }

        static void ShowAllHabbits()
        {
            Console.Clear();
            Console.WriteLine("ВАШИ ПРИВЫЧКИ");
            Console.WriteLine ("=".PadRight(50, '='));

            if(habits.Count == 0)
            {
                Console.WriteLine("\"Список привычек пуст. Добавьте первую привычку!");
                return;
            }

            for (int i = 0; i < habits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {habits[i]}");
            }
        }

        static void MarkHabitComplete()
        {
            ShowAllHabbits();

            if (habits.Count == 0)
            return;

            Console.Write("\nВведите номер привычки для отметки: ");
            if(int.TryParse(Console.ReadLine(), out int index) && index>=1 && index <= habits.Count)
            {
                habits[index - 1].MarkComplete();
            }
            else
            { 
                Console.WriteLine("Неверный номер привычки!");
            }
        }



        static void DeleteHabit()
        {
            ShowAllHabbits();

            if (habits.Count == 0)
            return;

            Console.Write("\nВведите номер привычки для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= habits.Count)
            {
                string habitName = habits[index - 1].Name;
                habits.RemoveAt(index-1);
                Console.WriteLine($"Привычка '{habitName}' удалена!");
            }
            else 
            {
                Console.WriteLine("Неверный номер привычки!");
            }

        }

        

    }
}