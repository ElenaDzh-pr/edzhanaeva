namespace ProjectDz;

class Program
{
    static string? name = null;
    static List<string> tasks = new List<string>();
    static int maxTaskLimit = 0;
    static int maxTaskLength = 0;
    static int maxLengthTask = 100;
    static int minLengthTask = 1;

    static void Main(string[] args)
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Введите максимально допустимое количество задач (от 1 до 100):");
                var input = Console.ReadLine();
                ValidateString(input);
                maxTaskLimit = ParseAndValidateInt(input, minLengthTask, maxLengthTask);
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Попробуйте еще раз.\n");
            }
        }

        while (true)
        {
            try
            {
                Console.WriteLine("Введите максимально допустимую длину задачи (от 1 до 100):");
                var input = Console.ReadLine();
                ValidateString(input);
                maxTaskLength = ParseAndValidateInt(input, minLengthTask, maxLengthTask);
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Попробуйте еще раз.\n");
            }
        }
            
        while (true)
        {
            try
            {
                ShowCurrentMenu();
                var command = Console.ReadLine();
                switch (command)
                {
                    case "/start": StartMessage(); break;
                    case "/help": Help(); break;
                    case "/info": Info(); break;
                    case "/echo": EchoMessage(); break;
                    case "/addtask": Addtask(); break;
                    case "/showtasks": Showtasks(); break;
                    case "/removetask": Removetask(); break;
                    case "/exit": Exit(); return;
                    default: Console.WriteLine("Неизвестная команда"); break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (TaskCountLimitException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (TaskLengthLimitException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DuplicateTaskException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла непредвиденная ошибка:");
                Console.WriteLine($"Тип: {ex.GetType()}");
                Console.WriteLine($"Сообщение: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine(ex.InnerException != null 
                    ? $"InnerException: {ex.InnerException.GetType()}: {ex.InnerException.Message}" 
                    : "InnerException: null");
            }
        }
    }

    static void ShowCurrentMenu()
    {
        Console.WriteLine(string.IsNullOrWhiteSpace(name)
            ? "Добро пожаловать в бота\nДоступные команды:"
            : $"{name}, доступные команды");

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("/start - начать работу");
        }

        if (!string.IsNullOrWhiteSpace(name))
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
        ValidateString(name);
    }

    static void EchoMessage()
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Команда недоступна. Введите имя через команду /start");
            return;
        }

        Console.WriteLine($"{name}, введите текст после команды /echo:");

        while (true)
        {
            string? input = Console.ReadLine() ?? "";
            ValidateString(input);
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

    static void Info()
    {
        Console.WriteLine(string.IsNullOrWhiteSpace(name)
            ? "Версия бота 1.0, дата создания 25.05.2025"
            : $"{name}, версия бота 1.0, дата создания 25.05.2025");
    }

    static void Addtask()
    {
        if (tasks.Count >= maxTaskLimit)
        {
            throw new TaskCountLimitException(maxTaskLimit);
        }
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Команда недоступна. Введите имя через команду /start");
            return;
        }

        Console.WriteLine($"{name}, пожалуйста, введите описание задачи:");

        string? taskDescription = Console.ReadLine()?.Trim();
        ValidateString(taskDescription);
        
        if (string.IsNullOrWhiteSpace(taskDescription))
        {
            Console.WriteLine($"{name}, описание задачи не может быть пустым");
            return;
        }
        
        if (taskDescription.Length > maxTaskLength)
        {
            throw new TaskLengthLimitException(taskDescription.Length, maxTaskLength);
        }

        if (tasks.Any(t => string.Equals(t, taskDescription, StringComparison.OrdinalIgnoreCase)))
        {
            throw new DuplicateTaskException(taskDescription);
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
        ValidateString(input);
        
        var i = !int.TryParse(input, out int taskNumber);

        if (taskNumber < 1 || taskNumber > tasks.Count)
        {
            Console.WriteLine($"{name}, ошибка, введите корректный номер от 1 до {tasks.Count}!");
            return;
        }

        tasks.RemoveAt(taskNumber - 1);
        Console.WriteLine($"Задача удалена. Осталось задач: {tasks.Count}");
    }

    static void Exit()
    {
        Console.WriteLine(string.IsNullOrEmpty(name)
            ? "До свидания!"
            : $"{name}, до свидания!");
    }
    
    public class TaskCountLimitException : Exception{
        public TaskCountLimitException() : base() { }
        public TaskCountLimitException(int maxTaskLimit) 
            : base($"Превышено максимальное количество задач: {maxTaskLimit}") { }
    }
    
    public class TaskLengthLimitException : Exception{
        public TaskLengthLimitException() : base() { }
        public TaskLengthLimitException(int taskLength, int maxTaskLength) 
            : base($"Длина задачи {taskLength} превышает максимально допустимое значение {maxTaskLength}") { }
    }
    
    public class DuplicateTaskException : Exception{
        public DuplicateTaskException() : base() { }
        public DuplicateTaskException(string task) 
            : base($"Задача {task} уже существует") { }
    }

    static int ParseAndValidateInt(string? str, int min, int max)
    {
        if (!int.TryParse(str, out int result) || result < min || result > max)
        {
            throw new ArgumentException($"Введите число в диапазоне от {min} до {max}");
        }
        return result;
    }

    static void ValidateString(string? str)
    {
        if (string.IsNullOrEmpty(str) || str.Trim().Length == 0)
        {
            throw new ArgumentException("Строка не может быть null, пустой или состоять только из пробелов");
        }
    }

}