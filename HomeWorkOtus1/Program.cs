using System;
using System.Collections.Generic;
using System.Text; // Подключаем пространство имён для кодировки

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8; // Устанавливаем кодировку UTF-8, (мой язык системы eng)
        string userName = "";
        bool isRunning = true;
        List<string> tasks = new List<string>();

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
                    Console.Write("Введите описание задачи: ");
                    string task = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(task))
                    {
                        tasks.Add(task);
                        Console.WriteLine("Задача добавлена!");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: задача не может быть пустой.");
                    }
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
}
