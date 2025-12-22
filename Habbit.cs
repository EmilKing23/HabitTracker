using System;

namespace HabitTracker
{
    public class Habit
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Streak { get; set; }

        // Конструктор без параметров - НЕОБХОДИМ для JSON-сериализации
        public Habit()
        {
            // Пустой конструктор
        }

        // Конструктор с параметрами
        public Habit(string name)
        {
            Name = name;
            CreatedDate = DateTime.Now;
            IsCompleted = false;
            Streak = 0;
        }

        // Метод для отметки выполнения привычки
        public void MarkComplete()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                Streak++;
                Console.WriteLine($"[V] Привычка '{Name}' выполнена! Серия: {Streak} дней.");
            }
            else
            {
                Console.WriteLine($"[i] Привычка '{Name}' уже была выполнена сегодня.");
            }
        }

        // Метод для сброса выполнения
        public void ResetCompletion()
        {
            IsCompleted = false;
            Console.WriteLine($"[R] Привычка '{Name}' готова к новому выполнению.");
        }

        // Переопределяем ToString() для красивого вывода
        public override string ToString()
        {
            string status = IsCompleted ? "[V] Выполнена" : "[ ] Ожидает";
            return $"{Name} | {status} | Создана: {CreatedDate:dd.MM.yy} | Серия: {Streak} дн.";
        }
    }
}