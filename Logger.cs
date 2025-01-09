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
        log?.Dispose(); // Close Stream
        log = File.OpenWrite(name + ".log");
        log.Write(new UTF8Encoding(true).GetBytes($"Vac'cuum log system\nStarted: {DateTime.Now}\n=============================================\n"));
    }

    ///<summary>
    ///Default logger constructor
    ///</summary>
    public Logger(string _name)
    {
        name = _name;
        Initialize();
    }

    // Destructor
    ~Logger()
    {
        log?.Dispose();
    }

    // Close log method
    public static void Close()
    {
        log?.Dispose();
        log = null;
    }

    private static void WriteToLog(byte[] data)
    {
        if (log == null)
        {
            Console.WriteLine("WARNING: INITIALIZATION FAILED!");
            return;
        }
        log.Write(data);
        log.Flush();
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
            
        message.Append($":: {level} ");
        
        if (includeStackTrace)
            message.Append($"from [{new System.Diagnostics.StackTrace().ToString()}]: \t");
            
        message.Append(content);
        message.Append("\n");
        
        return message.ToString();
    }

    ///<summary>
    ///Log method
    ///</summary>
    public static void Log(string content)
    {
        string message = FormatLogMessage("✉ [Log]", content);
        WriteToLog(new UTF8Encoding().GetBytes(message));
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(message);
        Console.ForegroundColor = defaultConsoleColor;
    }

    ///<summary>
    ///Info message method
    ///</summary>
    public static void Info(string content)
    {
        string message = FormatLogMessage("ℹ [Info]", content);
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
        string message = FormatLogMessage("😼 [Success]", content);
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
        string message = FormatLogMessage("⚠ [Warning]", text);
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
        string message = FormatLogMessage("😠 [Error]", _text);
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
        string message = FormatLogMessage("☠ [CRITICAL]", text);
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
        string message = FormatLogMessage("☠ [EXCEPTION]", 
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
