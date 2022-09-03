using System;

class Logger
{
    /// <summary>Процедура логирования</summary>
    internal static void Log(string type, string module, string message)
    {
        string time = DateTime.Now.ToString();
        string logMessage = $"{time} - [ {type} ] - {module} - {message}";
        Console.WriteLine(logMessage);
        try
        {
            LocalWorker.LogInFile(logMessage);
        }
        catch (Exception)
        {
            throw;
        }
    }
}