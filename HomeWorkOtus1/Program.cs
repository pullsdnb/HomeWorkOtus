// Подключаем пространства имён
using System;
using System.Collections.Generic;
using System.Text;

// Основной класс программы
class Program
{
    // Точка входа в программу
    static void Main()
    {
        try
        {
            // Устанавливаем кодировку консоли для корректного отображения русских символов
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Переменные пользователя и флаги состояния
            string userName = "";
            bool isRunning = true;
            List<string> tasks = new List<string>();

            // Ввод ограничений от пользователя
            int taskCountLimit = GetLimitFromUser("Введите максимально допустимое количество задач (1–100): ", 1, 100);
            int taskLengthLimit = GetLimitFromUser("Введите максимально допустимую длину задачи (1–100): ", 1, 100);

            // Приветствие и справка
            ShowGreeting();

            // Основной цикл работы бота
            while (isRunning)
            {
                Console.Write("\nВведите команду: ");
                string? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Ошибка: введите команду.");
                    continue;
                }

                string[] parts = input.Split(' ', 2);
                string command = parts[0].ToLower();
                string argument = parts.Length > 1 ? parts[1] : "";

                switch (command)
                {
                    case "/start":
                        userName = GetUserName();
                        break;

                    case "/help":
                        ShowHelp();
                        break;

                    case "/info":
                        ShowInfo();
                        break;

                    case "/echo":
                        Echo(argument, userName);
                        break;

                    case "/addtask":
                        AddTask(tasks, taskCountLimit, taskLengthLimit);
                        break;

                    case "/showtasks":
                        ShowTasks(tasks);
                        break;

                    case "/removetask":
                        RemoveTask(tasks);
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
            Console.WriteLine($"Произошла ошибка: {ex.GetType().Name} — {ex.Message}");
        }
    }

    // Метод для отображения приветствия
    static void ShowGreeting()
    {
        Console.WriteLine("Привет! Это консольный бот с задачами.");
        ShowHelp();
    }

    // Метод для отображения справки
    static void ShowHelp()
    {
        Console.WriteLine("\nДоступные команды:");
        Console.WriteLine("/start - ввести имя");
        Console.WriteLine("/help - показать справку");
        Console.WriteLine("/info - информация о программе");
        Console.WriteLine("/echo [текст] - повторить ваш текст");
        Console.WriteLine("/addtask - добавить задачу");
        Console.WriteLine("/showtasks - показать задачи");
        Console.WriteLine("/removetask - удалить задачу");
        Console.WriteLine("/exit - выход из программы");
    }

    // Метод для отображения информации о программе
    static void ShowInfo()
    {
        Console.WriteLine("Версия программы: 2.0");
        Console.WriteLine($"Дата запуска: {DateTime.Now:yyyy-MM-dd}");
    }

    // Метод для ввода имени пользователя
    static string GetUserName()
    {
        Console.Write("Введите ваше имя: ");
        string? name = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Ошибка: имя не может быть пустым.");
            return GetUserName();
        }

        Console.WriteLine($"Приятно познакомиться, {name}!");
        return name;
    }

    // Метод для повтора текста
    static void Echo(string text, string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Ошибка: сначала введите команду /start.");
            return;
        }

        Console.WriteLine($"{userName}, ты сказал: {text}");
    }

    // Метод для получения ограничения от пользователя
    static int GetLimitFromUser(string message, int min, int max)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int result) || result < min || result > max)
        {
            Console.WriteLine($"Ошибка: введите число от {min} до {max}.");
            return GetLimitFromUser(message, min, max);
        }

        return result;
    }

    // Метод для добавления задачи
    static void AddTask(List<string> tasks, int maxTasks, int maxLength)
    {
        if (tasks.Count >= maxTasks)
            throw new TaskCountLimitException(maxTasks);

        Console.Write("Введите описание задачи: ");
        string? task = Console.ReadLine()?.Trim();

        ValidateTask(task, maxLength, tasks);

        tasks.Add(task!);
        Console.WriteLine("Задача добавлена!");
    }

    // Метод для проверки задачи
    static void ValidateTask(string? task, int maxLength, List<string> tasks)
    {
        if (string.IsNullOrWhiteSpace(task))
            throw new ArgumentException("Ошибка: задача не может быть пустой.");

        if (task.Length > maxLength)
            throw new TaskLengthLimitException(task.Length, maxLength);

        if (tasks.Contains(task))
            throw new DuplicateTaskException(task);
    }

    // Метод для отображения задач
    static void ShowTasks(List<string> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        Console.WriteLine("Ваши задачи:");
        for (int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i]}");
        }
    }

    // Метод для удаления задачи
    static void RemoveTask(List<string> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        ShowTasks(tasks);
        Console.Write("Введите номер задачи для удаления: ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int number) && number >= 1 && number <= tasks.Count)
        {
            Console.WriteLine($"Удалена задача: {tasks[number - 1]}");
            tasks.RemoveAt(number - 1);
        }
        else
        {
            Console.WriteLine("Ошибка: введите корректный номер задачи.");
        }
    }
}

// Пользовательские исключения

// Исключение при превышении лимита количества задач
public class TaskCountLimitException : Exception
{
    public TaskCountLimitException(int limit)
        : base($"Ошибка: превышено допустимое количество задач — {limit}.")
    { }
}

// Исключение при превышении длины задачи
public class TaskLengthLimitException : Exception
{
    public TaskLengthLimitException(int actualLength, int maxLength)
        : base($"Ошибка: длина задачи ({actualLength}) превышает максимально допустимую ({maxLength}).")
    { }
}

// Исключение при попытке добавить дубликат
public class DuplicateTaskException : Exception
{
    public DuplicateTaskException(string task)
        : base($"Ошибка: задача '{task}' уже существует.")
    { }
}
