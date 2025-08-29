// Подключаем пространство имён
using System;
using System.Collections.Generic;

// Пространство имён проекта
namespace ConsoleBotWithTasks
{
    // Кастомные исключения
    public class TaskCountLimitException : Exception
    {
        public TaskCountLimitException(string message) : base(message) { }
    }

    public class TaskLengthLimitException : Exception
    {
        public TaskLengthLimitException(string message) : base(message) { }
    }

    public class DuplicateTaskException : Exception
    {
        public DuplicateTaskException(string message) : base(message) { }
    }

    // Класс с точкой входа
    class Program
    {
        // Список задач
        private static List<string> tasks = new List<string>();

        // Лимиты
        private static int maxTaskCount = 0;
        private static int maxTaskLength = 0;

        // Входная точка
        static void Main(string[] args)
        {
            try
            {
                // Настройка ограничений
                maxTaskCount = GetLimitFromUser("Введите максимально допустимое количество задач (1–100): ", 1, 100);
                maxTaskLength = GetLimitFromUser("Введите максимально допустимую длину задачи (1–100): ", 1, 100);

                Console.WriteLine("Бот запущен. Введите команду:");
                while (true)
                {
                    string? input = Console.ReadLine();
                    if (input == null) continue;

                    switch (input.Trim().ToLower())
                    {
                        case "/start":
                            Start();
                            break;
                        case "/help":
                            Help();
                            break;
                        case "/info":
                            Info();
                            break;
                        case "/addtask":
                            AddTask();
                            break;
                        case "/showtasks":
                            ShowTasks();
                            break;
                        case "/removetask":
                            RemoveTask();
                            break;
                        case "/exit":
                            Console.WriteLine("Бот завершил работу.");
                            return;
                        default:
                            Echo(input);
                            break;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка в аргументе: {ex.Message}");
            }
            catch (TaskCountLimitException ex)
            {
                Console.WriteLine($"Превышен лимит задач: {ex.Message}");
            }
            catch (TaskLengthLimitException ex)
            {
                Console.WriteLine($"Слишком длинная задача: {ex.Message}");
            }
            catch (DuplicateTaskException ex)
            {
                Console.WriteLine($"Дубликат задачи: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.GetType().Name} — {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
                }
            }
        }

        // Метод получения числового лимита с проверкой
        static int GetLimitFromUser(string message, int min, int max)
        {
            Console.Write(message);
            string? input = Console.ReadLine();
            return ParseAndValidateInt(input, min, max);
        }

        // Метод для проверки и парсинга числа
        static int ParseAndValidateInt(string? input, int min, int max)
        {
            if (!int.TryParse(input, out int value))
                throw new ArgumentException("Введено не число.");

            if (value < min || value > max)
                throw new ArgumentException($"Число должно быть в диапазоне от {min} до {max}.");

            return value;
        }

        // Метод проверки строки
        static void ValidateString(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Ввод не должен быть пустым.");

            if (input.Length > maxTaskLength)
                throw new TaskLengthLimitException($"Задача слишком длинная. Макс: {maxTaskLength} символов.");
        }

        // Команды

        static void Start()
        {
            Console.WriteLine("Добро пожаловать! Введите /help для списка команд.");
        }

        static void Help()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("/start — приветствие");
            Console.WriteLine("/help — справка");
            Console.WriteLine("/info — информация");
            Console.WriteLine("/addtask — добавить задачу");
            Console.WriteLine("/showtasks — показать задачи");
            Console.WriteLine("/removetask — удалить задачу");
            Console.WriteLine("/exit — выход");
        }

        static void Info()
        {
            Console.WriteLine($"Всего задач: {tasks.Count}, лимит: {maxTaskCount}, макс. длина: {maxTaskLength}");
        }

        static void AddTask()
        {
            if (tasks.Count >= maxTaskCount)
                throw new TaskCountLimitException("Нельзя добавить больше задач.");

            Console.Write("Введите текст задачи: ");
            string? task = Console.ReadLine();

            ValidateString(task);

            if (tasks.Contains(task!))
                throw new DuplicateTaskException("Такая задача уже есть.");

            tasks.Add(task!);
            Console.WriteLine("Задача добавлена.");
        }

        static void ShowTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("Ваши задачи:");
            for (int i = 0; i < tasks.Count; i++)
                Console.WriteLine($"{i + 1}. {tasks[i]}");
        }

        static void RemoveTask()
        {
            ShowTasks();
            Console.Write("Введите номер задачи для удаления: ");
            string? input = Console.ReadLine();
            int index = ParseAndValidateInt(input, 1, tasks.Count);

            string removed = tasks[index - 1];
            tasks.RemoveAt(index - 1);
            Console.WriteLine($"Задача \"{removed}\" удалена.");
        }

        static void Echo(string message)
        {
            Console.WriteLine($"Вы сказали: {message}");
        }
    }
}
