using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker
{
    [Table("Habits")]
    public class Habit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Streak { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastCompletedDate { get; set; }

        // Конструктор без параметров - НЕОБХОДИМ для JSON-сериализации
        public Habit()
        {
            CreatedDate= DateTime.Now;
            IsCompleted= false;
            Streak= 0;
        }

        // Конструктор с параметрами
        public Habit(string name) : this()
        {
            Name = name;
        }

        // Метод для отметки выполнения привычки
        public void MarkComplete()
        {
            var today = DateTime.Now.Date;

            if (!IsCompleted && LastCompletedDate?.Date == today)
            {
                Console.WriteLine($"[i] Привычка '{Name}' уже была выполнена сегодня.");
                return;
            }

            if (LastCompletedDate?.Date == today.AddDays(-1))
            {
                Streak++;
            }
            else if (LastCompletedDate?.Date < today.AddDays(-1))
            {
                Streak = 1;
            }
            else
            {
                Streak = 1;
            }

            IsCompleted = true;
            LastCompletedDate = today;

            Console.WriteLine($"[V] Привычка '{Name}' выполнена! Серия: {Streak} дней.");
        }

        // Метод для сброса выполнения
        public void ResetCompletion()
        {
           if(LastCompletedDate?.Date < DateTime.Today)
            {                  
                IsCompleted = false;
                Console.WriteLine($"[R] Привычка '{Name}' готова к новому выполнению.");
            }
        }

        // Переопределяем ToString() для красивого вывода
        public override string ToString()
        {
            string status = IsCompleted ? "[V] Выполнена" : "[ ] Ожидает";
            string lastCompleted = LastCompletedDate.HasValue
            ? $"Последний раз: {LastCompletedDate.Value:dd.MM.yy}"
            : "Никогда не выполнялась";

            return $"{Name} | {status} | Создана: {CreatedDate:dd.MM.yy} | Серия: {Streak} дн. | {LastCompletedDate}";
            ;
        }
    }
}