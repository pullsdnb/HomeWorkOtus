using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Основной класс Stack
public class Stack
{
    // Вложенный класс StackItem — элемент стека (только доступен внутри Stack)
    private class StackItem
    {
        public string Value;        // Значение элемента
        public StackItem Previous;  // Ссылка на предыдущий элемент

        public StackItem(string value, StackItem previous)
        {
            Value = value;
            Previous = previous;
        }
    }

    private StackItem top; // Верхний элемент стека
    private int size;      // Количество элементов в стеке

    // Конструктор, который может принимать любое количество строковых параметров
    public Stack(params string[] items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    // Метод добавления элемента в стек (сверху)
    public void Add(string item)
    {
        top = new StackItem(item, top); // Новый элемент ссылается на предыдущий верхний
        size++;                         // Увеличиваем размер стека
    }

    // Метод извлечения верхнего элемента из стека
    public string Pop()
    {
        if (top == null)
            throw new InvalidOperationException("Стек пустой");

        string value = top.Value; // Сохраняем значение верхнего элемента
        top = top.Previous;       // Сдвигаем верх вниз
        size--;                  // Уменьшаем размер
        return value;             // Возвращаем значение
    }

    // Свойство, показывающее сколько элементов в стеке
    public int Size => size;

    // Свойство, возвращающее значение верхнего элемента или null, если стек пустой
    public string Top => top?.Value;

    // Метод для получения всех элементов стека сверху вниз (в виде списка строк)
    public List<string> GetElements()
    {
        var elements = new List<string>();
        var current = top;
        while (current != null)
        {
            elements.Add(current.Value);
            current = current.Previous;
        }
        return elements;
    }

    // Статический метод Concat (Доп. задание 2)
    public static Stack Concat(params Stack[] stacks)
    {
        var result = new Stack();

        foreach (var stack in stacks)
        {
            // Получаем элементы стека в списке сверху вниз
            var elems = stack.GetElements();

            // Добавляем в result элементы в обратном порядке (чтобы верхний был последний)
            for (int i = 0; i < elems.Count; i++)
            {
                result.Add(elems[i]);
            }
        }

        return result;
    }

    // Для удобного вывода в консоль — элементы снизу вверх, через запятую
    public override string ToString()
    {
        var elements = GetElements();
        elements.Reverse(); // чтобы снизу вверх
        return string.Join(", ", elements);
    }
}

// Класс расширения StackExtensions с методом расширения Merge (Доп. задание 1)
public static class StackExtensions
{
    // Метод расширения Merge: добавляет все элементы из s2 в s1 в обратном порядке
    public static void Merge(this Stack s1, Stack s2)
    {
        var elems = s2.GetElements();

        // Добавляем элементы s2 в s1 в обратном порядке
        for (int i = elems.Count - 1; i >= 0; i--)
        {
            s1.Add(elems[i]);
        }
    }
}

// Класс для запуска и проверки
class Program
{
    static void Main(string[] args)
    {
        // Чтобы консоль могла корректно выводить русский текст:
        Console.OutputEncoding = Encoding.UTF8;

        // Основное задание: создаём стек с элементами "a", "b", "c"
        var s = new Stack("a", "b", "c");

        Console.WriteLine($"Размер стека = {s.Size}, Верхний элемент = '{s.Top}'");

        var deleted = s.Pop();
        Console.WriteLine($"Извлек верхний элемент '{deleted}', размер стека = {s.Size}");

        s.Add("d");
        Console.WriteLine($"Размер стека = {s.Size}, Верхний элемент = '{s.Top}'");

        s.Pop();
        s.Pop();
        s.Pop();
        Console.WriteLine($"Размер стека = {s.Size}, Верхний элемент = {(s.Top == null ? "null" : s.Top)}");

        // Попытка извлечь элемент из пустого стека — покажет ошибку
        try
        {
            s.Pop();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\nДоп. задание 1: Merge");

        var s1 = new Stack("a", "b", "c");
        var s2 = new Stack("1", "2", "3");

        s1.Merge(s2);
        Console.WriteLine("После Merge стека s1 и s2 элементы стека s1:");
        Console.WriteLine(s1); // Ожидаем: a, b, c, 3, 2, 1

        Console.WriteLine("\nДоп. задание 2: Concat");

        var s3 = Stack.Concat(
            new Stack("a", "b", "c"),
            new Stack("1", "2", "3"),
            new Stack("А", "Б", "В")
        );

        Console.WriteLine("После Concat элементов трёх стеков:");
        Console.WriteLine(s3); // Ожидаем: c, b, a, 3, 2, 1, В, Б, А

        // Доп. задание 3 реализовано через использование StackItem (вложенный класс), доступный только внутри Stack
        // Это уже сделано — StackItem создан внутри Stack и нигде снаружи его нельзя создать.
    }
}
