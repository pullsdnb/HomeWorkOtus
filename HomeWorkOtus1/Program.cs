using System;
using System.Collections.Generic;
using System.Text; // Подключаем пространство имён для кодировки

class Program
{
    static void Main()
    {
        try
        {
            // Устанавливаем кодировку UTF-8 для правильного отображения русских символов
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            string userName = "";
            bool isRunning = true;
            List<string> tasks = new List<string>();

            // Вводим максимальные параметры
            int taskCountLimit = 0;
            Console.Write("Введите максимально допустимое количество задач: ");
            taskCountLimit = ParseAndValidateInt(Console.ReadLine(), 1, 100);

            int taskLengthLimit = 0;
            Console.Write("Введите максимально допустимую длину задачи: ");
            taskLengthLimit = ParseAndValidateInt(Console.ReadLine(), 1, 100);

            Console.WriteLine("Привет! Это консольный бот с задачами.");
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("/start - ввести имя");
            Console.WriteLine("/help - справка");
            Console.WriteLine("/info - информация о программе");
            Console.WriteLine("/echo [текст] - повторить ваш текст");
            Console.WriteLine("/addtask - добавить задачу");
            Console.WriteLine("/showtasks - показать список задач");
            Console.WriteLine("/removetask - удалить задачу");
            Console.WriteLine("/exit - выход");

            while (isRunning)
            {
                Console.Write("\nВведите команду: ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Ошибка: введите команду!");
                    continue;
                }

                string[] parts = input.Split(' ', 2);
                string command = parts[0].ToLower();
                string argument = parts.Length > 1 ? parts[1] : "";

                switch (command)
                {
                    case "/start":
                        Console.Write("Введите ваше имя: ");
                        userName = Console.ReadLine()?.Trim();
                        Console.WriteLine($"Приятно познакомиться, {userName}!");
                        break;

                    case "/help":
                        Console.WriteLine("Команды: /start, /help, /info, /echo [текст], /addtask, /showtasks, /removetask, /exit.");
                        break;

                    case "/info":
                        Console.WriteLine("Версия программы: 1.0.1");
                        Console.WriteLine($"Дата создания: {DateTime.Now:yyyy-MM-dd}");
                        break;

                    case "/echo":
                        if (!string.IsNullOrEmpty(userName))
                        {
                            Console.WriteLine($"{userName}, ты сказал: {argument}");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: введите /start перед использованием /echo!");
                        }
                        break;

                    case "/addtask":
                        if (tasks.Count >= taskCountLimit)
                        {
                            throw new TaskCountLimitException(taskCountLimit);
                        }

                        Console.Write("Введите описание задачи: ");
                        string task = Console.ReadLine()?.Trim();
                        ValidateString(task);

                        if (task.Length > taskLengthLimit)
                        {
                            throw new TaskLengthLimitException(task.Length, taskLengthLimit);
                        }

                        if (tasks.Contains(task))
                        {
                            throw new DuplicateTaskException(task);
                        }

                        tasks.Add(task);
                        Console.WriteLine("Задача добавлена!");
                        break;

                    case "/showtasks":
                        if (tasks.Count > 0)
                        {
                            Console.WriteLine("Ваши задачи:");
                            for (int i = 0; i < tasks.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {tasks[i]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Список задач пуст.");
                        }
                        break;

                    case "/removetask":
                        if (tasks.Count == 0)
                        {
                            Console.WriteLine("Ошибка: список задач пуст.");
                            break;
                        }

                        Console.WriteLine("Ваши задачи:");
                        for (int i = 0; i < tasks.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {tasks[i]}");
                        }

                        Console.Write("Введите номер задачи для удаления: ");
                        string taskNumberInput = Console.ReadLine()?.Trim();
                        if (int.TryParse(taskNumberInput, out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
                        {
                            tasks.RemoveAt(taskNumber - 1);
                            Console.WriteLine("Задача удалена.");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: введите корректный номер задачи.");
                        }
                        break;

                    case "/exit":
                        Console.WriteLine("Выход из программы...");
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда. Введите /help для справки.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: {ex.GetType()}, {ex.Message}\nStackTrace: {ex.StackTrace}\nInnerException: {ex.InnerException}");
        }
    }

    static void ValidateString(string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentException("Ошибка: строка не может быть пустой или состоять только из пробелов.");
        }
    }

    static int ParseAndValidateInt(string? str, int min, int max)
    {
        if (!int.TryParse(str, out int result) || result < min || result > max)
        {
            throw new ArgumentException($"Ошибка: введено некорректное число. Оно должно быть в пределах от {min} до {max}.");
        }
        return result;
    }
}

// Исключения для обработки ошибок

public class TaskCountLimitException : Exception
{
    public TaskCountLimitException(int taskCountLimit)
        : base($"Превышено максимальное количество задач равное {taskCountLimit}")
    {
    }
}

public class TaskLengthLimitException : Exception
{
    public TaskLengthLimitException(int taskLength, int taskLengthLimit)
        : base($"Длина задачи '{taskLength}' превышает максимально допустимое значение {taskLengthLimit}")
    {
    }
}

public class DuplicateTaskException : Exception
{
    public DuplicateTaskException(string task)
        : base($"Задача '{task}' уже существует")
    {
    }
}
