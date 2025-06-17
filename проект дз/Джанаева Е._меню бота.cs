namespace ProjectDz;

class Program
{
    static string? name = null;
    static List<string> tasks = new List<string>();

    static void Main(string[] args)
    {
        while (true)
        {
            ShowCurrentMenu();
            var command = Console.ReadLine();

            switch (command)
            {
                case "/start":
                    StartMessage();
                    break;
                case "/help":
                    Help();
                    break;
                case "/info":
                    Console.WriteLine(string.IsNullOrEmpty(name)
                        ? "Версия бота 1.0, дата создания 25.05.2025"
                        : $"{name}, версия бота 1.0, дата создания 25.05.2025");
                    break;
                case "/echo":
                    EchoMessage();
                    break;
                case "/addtask":
                    Addtask();
                    break;
                case "/showtasks":
                    Showtasks();
                    break;
                case "/removetask":
                    Removetask();
                    break;
                case "/exit":
                    Console.WriteLine(string.IsNullOrEmpty(name)
                        ? "До свидания!"
                        : $"{name}, до свидания!");
                    return;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }
    }

    static void ShowCurrentMenu()
    {
        Console.WriteLine(string.IsNullOrEmpty(name)
            ? "Добро пожаловать в бота\nДоступные команды:"
            : $"{name}, доступные команды");

        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("/start - начать работу");
        }

        if (!string.IsNullOrEmpty(name))
        {
            Console.WriteLine("/echo - отправить текст");
            Console.WriteLine("/addtask - добавить задачу");
            Console.WriteLine("/showtasks - показать список задач");
            Console.WriteLine("/removetask - удалить задачу");
        }

        Console.WriteLine("/help - справка");
        Console.WriteLine("/info - информация о боте");
        Console.WriteLine("/exit - выход");
        Console.Write("Введите команду: ");
    }

    static void StartMessage()
    {
        Console.WriteLine("Пожалуйста, введите ваше имя: ");
        name = Console.ReadLine();
    }

    static void EchoMessage()
    {
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Команда недоступна. Введите имя через команду /start");
            return;
        }
        Console.WriteLine($"{name}, введите текст после команды /echo:");

        while (true)
        {
            string? input = Console.ReadLine() ?? "";

            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length >= 2)
            {
                string result = string.Join(' ', words.Skip(1));
                Console.WriteLine($"{result}");
                return;
            }
            else
            {
                Console.WriteLine("Введите текст через команду /echo Ваш текст");
            }
        }
    }

    static void Help()
    {
        Console.WriteLine($"Описание доступных команд:");
        Console.WriteLine("/start - начало работы с ботом, ввод имени");
        Console.WriteLine("/echo - повторяет введенное сообщение и отправляет его обратно");
        Console.WriteLine("/info - получить информацию о боте");
        Console.WriteLine("/addtask - добавить задачу в список задач");
        Console.WriteLine("/showtasks - показать список введенных задач");
        Console.WriteLine("/removetask - удалить задачу из текущего списка");
    }

    static void Addtask()
    {
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Команда недоступна. Введите имя через команду /start");
            return;
        }
        Console.WriteLine($"{name}, пожалуйста, введите описание задачи:");

        string? taskDescription = Console.ReadLine()?.Trim(); ;
        if (string.IsNullOrWhiteSpace(taskDescription))
        {
            Console.WriteLine($"{name}, описание задачи не может быть пустым");
            return;
        }
        tasks.Add(taskDescription);

        Console.WriteLine($"{name}, задача добавлена!");
    }

    static void Showtasks()
    {
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Команда недоступна. Введите имя через команду /start");
            return;
        }

        if (tasks.Count == 0)
        {
            Console.WriteLine($"{name}, список задач пуст");
            return;
        }
        else
        {
            Console.WriteLine("\nТекущий список задач:");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }
    }

    static void Removetask()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine($"{name}, список задач пуст. Нечего удалять.");
            return;
        }

        Showtasks();

        Console.WriteLine($"{name}, введите номер задачи для удаления:");
        var input = Console.ReadLine();
        var i = !int.TryParse(input, out int taskNumber);

        if (taskNumber < 1 || taskNumber > tasks.Count)
        {
            Console.WriteLine($"{name}, ошибка, введите корректный номер от 1 до {tasks.Count}!");
            return;
        }

        tasks.RemoveAt(taskNumber - 1);
        Console.WriteLine($"Задача удалена. Осталось задач: {tasks.Count}");
        
    }

}
