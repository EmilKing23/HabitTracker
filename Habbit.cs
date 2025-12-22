using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    /// <summary>
    /// Класс, представляющий привычку
    /// </summary>
    public class Habit
    {
        // Свойства (данные привычки)
        public string Name { get; set; }           // Название привычки
        public DateTime CreatedDate { get; set; }  // Дата создания
        public bool IsCompleted { get; set; }      // Выполнена ли привычка
        public int Streak { get; set; }           // Текущий счётчик последовательных дней

        // Конструктор - вызывается при создании новой привычки
        public Habit(string name)
        {
            Name = name;
            CreatedDate = DateTime.Now;  // Устанавливаем текущую дату и время
            IsCompleted = false;         // По умолчанию привычка не выполнена
            Streak = 0;                  // Начинаем с нуля
        }

        // Метод для отметки выполнения привычки
        public void MarkComplete()
        {
            if (!IsCompleted)  // Проверяем, не выполнена ли уже привычка
            {
                IsCompleted = true;
                Streak++;      // Увеличиваем счётчик
                Console.WriteLine($" Привычка '{Name}' выполнена! Серия: {Streak} дней.");
            }
            else
            {
                Console.WriteLine($" Привычка '{Name}' уже была выполнена сегодня.");
            }
        }

        // Метод для сброса выполнения (например, на следующий день)
        public void ResetCompletion()
        {
            IsCompleted = false;
            Console.WriteLine($" Привычка '{Name}' готова к новому выполнению.");
        }

        // Переопределяем ToString() для красивого вывода
        public override string ToString()
        {
            string status = IsCompleted ? "Выполнена" : "Ожидает";
            return $"{Name} | {status} | Создана: {CreatedDate:dd.MM.yy} | Серия: {Streak} дн.";
        }
    }
}



