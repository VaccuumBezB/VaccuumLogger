using System.Net;
using System.IO;
using System.Text;

namespace VaccuumLogistics;

public class Logger
{
    public static string name = "";
    private static FileStream? log;
    private static bool includeTimestamp = true;
    private static bool includeStackTrace = true;
    private static ConsoleColor defaultConsoleColor = Console.ForegroundColor;
    private static bool autoOpenOnCritical = true;
    
    private void Initialize()
    {
        log?.Dispose(); // Закрываем старый поток, если он был
        log = File.OpenWrite(name + ".log");
        log.Write(new UTF8Encoding(true).GetBytes($"Vac'cuum log system\nStarted: {DateTime.Now}\n=============================================\n"));
    }

    ///<summary>
    ///Стандартный конструктор логгера, укажите имя в скобках
    ///</summary>
    public Logger(string _name)
    {
        name = _name;
        Initialize();
    }

    // Добавляем деструктор для освобождения ресурсов
    ~Logger()
    {
        log?.Dispose();
    }

    // Добавляем метод для принудительного закрытия лога
    public static void Close()
    {
        log?.Dispose();
        log = null;
    }

    // В остальных методах заменяем прямое обращение к log на проверку
    private static void WriteToLog(byte[] data)
    {
        if (log == null)
        {
            Console.WriteLine("ВНИМАНИЕ: Логгер не инициализирован!");
            return;
        }
        log.Write(data);
        log.Flush(); // Принудительно записываем в файл
    }

    public static void Configure(bool showTimestamp = true, bool showStackTrace = true, bool openLogsOnCritical = true)
    {
        includeTimestamp = showTimestamp;
        includeStackTrace = showStackTrace;
        autoOpenOnCritical = openLogsOnCritical;
    }

    private static string FormatLogMessage(string level, string content)
    {
        StringBuilder message = new StringBuilder();
        
        if (includeTimestamp)
            message.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} ");
            
        message.Append($"----- {level} ");
        
        if (includeStackTrace)
            message.Append($"from [{new System.Diagnostics.StackTrace().ToString()}]: \n\t");
            
        message.Append(content);
        message.Append("\n");
        
        return message.ToString();
    }

    ///<summary>
    ///Метод обычного отчёта
    ///</summary>
    public static void Log(string content)
    {
        string message = FormatLogMessage("0xF0 Log", content);
        WriteToLog(new UTF8Encoding().GetBytes(message));
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(message);
        Console.ForegroundColor = defaultConsoleColor;
    }

    ///<summary>
    ///Метод информационного сообщения
    ///</summary>
    public static void Info(string content)
    {
        string message = FormatLogMessage("0xF1 Info", content);
        WriteToLog(new UTF8Encoding().GetBytes(message));
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(message);
        Console.ForegroundColor = defaultConsoleColor;
    }

    ///<summary>
    ///Метод успешного выполнения
    ///</summary>
    public static void Success(string content)
    {
        string message = FormatLogMessage("0xF2 Success", content);
        WriteToLog(new UTF8Encoding().GetBytes(message));
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(message);
        Console.ForegroundColor = defaultConsoleColor;
    }

    ///<summary>
    ///Метод предупреждения
    ///</summary>
    public static void Warn(string text)
    {
        string message = FormatLogMessage("0xE2 Warning", text);
        WriteToLog(new UTF8Encoding().GetBytes(message));
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(message);
        Console.ForegroundColor = defaultConsoleColor;
    }

    ///<summary>
    ///Метод ошибки
    ///</summary>
    public static void Error(string _text)
    {
        string message = FormatLogMessage("0xE1 Error", _text);
        WriteToLog(new UTF8Encoding().GetBytes(message));
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(message);
        Console.ForegroundColor = defaultConsoleColor;
    }

    ///<summary>
    ///Метод критической ошибки
    ///</summary>
    public static void Critical(string text, bool openLog = true)
    {
        string message = FormatLogMessage("0xE0 CRITICAL", text);
        WriteToLog(new UTF8Encoding().GetBytes(message));
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.BackgroundColor = ConsoleColor.White;
        Console.Write(message);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = defaultConsoleColor;
        
        if (autoOpenOnCritical) OpenLog();
    }

    ///<summary>
    ///Метод для логирования исключений
    ///</summary>
    public static void Exception(Exception ex, bool openLog = true)
    {
        string message = FormatLogMessage("0xE0 EXCEPTION", 
            $"{ex.Message}\nStackTrace:\n{ex.StackTrace}");
        WriteToLog(new UTF8Encoding().GetBytes(message));
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.BackgroundColor = ConsoleColor.White;
        Console.Write(message);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = defaultConsoleColor;
        
        if (openLog) OpenLog();
    }

    public static void OpenLog()
    {
        System.Diagnostics.Process.Start(name + ".log");
    }
}
