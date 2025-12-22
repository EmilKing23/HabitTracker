using System;
using System.Collections.Generic;

namespace HabitTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🚀 Добро пожаловать в трекер привычек!");
            Console.WriteLine("Введите ваше имя:");
            string userName = Console.ReadLine();

            Console.WriteLine($"\nОтлично, {userName}! Давайте начнем.");

            List<string> habits = new List<string>();
            string input;

            do
            {
                Console.WriteLine("\nВведите название привычки (или 'stop' для завершения):");
                input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) && input.ToLower() != "stop")
                {
                    habits.Add(input);
                    Console.WriteLine($"Привычка '{input}' добавлена!");
                }

            } while (input?.ToLower() != "stop");

            Console.WriteLine("\n📋 Ваш список привычек:");
            if (habits.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                for (int i = 0; i < habits.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {habits[i]}");
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}