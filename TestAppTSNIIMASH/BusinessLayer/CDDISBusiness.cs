using System;
using System.Collections.Generic;
using System.IO;

class CDDISBusiness
{
    /// <summary>Метод скачивания данных из сети</summary>
    internal static void DownloadData(string savePath, string fileName, string username, string password, int defTriesToConnect)
    {
        Logger.Log("Debug", "Скачивание файла CDDIS", "Скачивание данных запущено");
        WebWorker.CreateVariables();
        try
        {
            string data = GetResultFromNet(username, password, defTriesToConnect);
            LocalWorker.SaveData(data, savePath, fileName);
            Logger.Log("Debug", "Скачивание файла CDDIS", $"Данные успешно сохранены по пути {savePath}\\{fileName}");
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Скачивание файла CDDIS", $"Ошибка при скачивании файла\n {e.Message}");
            throw;
        }
    }
    /// <summary>Метод скачивания данных из сети и их разбора</summary>
    internal static List<EopParam> DownloadAndRefactorData(string username, string password, int defTriesToConnect)
    {
        Logger.Log("Debug", "Скачивание и обработка CDDIS", "Скачивание и обработка данных запущена");
        WebWorker.CreateVariables();
        try
        {
            string data = GetResultFromNet(username, password, defTriesToConnect);
            List<EopParam> EopParams = ProcessResult(data);
            Logger.Log("Debug", "Скачивание и обработка CDDIS", "Данные успешно скачаны и обработаны");
            return EopParams;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Скачивание и обработка CDDIS", $"Ошибка при скачивании и обработке \n{e.Message}");
            throw;
        }
    }
    /// <summary>Метод разбора данных из файла</summary>
    internal static List<EopParam> RefactorData(string savePath, string fileName)
    {
        Logger.Log("Debug", "Обработка файла CDDIS", "Обработка файла запущена");
        try
        {
            string data = LocalWorker.ReadData(savePath, fileName);
            Logger.Log("Debug", "Обработка файла CDDIS", $"Данные успешно считаны из файла {savePath}\\{fileName}");
            List<EopParam> EopParams = ProcessResult(data);
            return EopParams;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Обработка файла CDDIS", $"Ошибка при обработке файла\n {e.Message}");
            throw;
        }
    }
    /// <summary>Метод тестирования</summary>
    internal static void Test(string username, string password, int defTriesToConnect)
    {
        Logger.Log("Debug", "Тестирование CDDIS", "Тестирование запущено");
        WebWorker.CreateVariables();
        try
        {
            Logger.Log("Debug", "Тестирование CDDIS", "Тестирование доступа к данным в сети");
            string data = GetResultFromNet(username, password, defTriesToConnect);
            Logger.Log("Debug", "Тестирование CDDIS", "Данные успешно получены из сети");
            Logger.Log("Debug", "Тестирование CDDIS", "Тестирование сохранения данных");
            string testFilePath = Directory.GetCurrentDirectory();
            string testFileName = "test.txt";
            LocalWorker.SaveData(data, testFilePath, testFileName);
            Logger.Log("Debug", "Тестирование CDDIS", "Данные успешно сохранены в файл");
            Logger.Log("Debug", "Тестирование CDDIS", "Тестирование получение данных из файла");
            data = LocalWorker.ReadData(testFilePath, testFileName);
            Logger.Log("Debug", "Тестирование CDDIS", $"Данные успешно считаны из файла");
            Logger.Log("Debug", "Тестирование CDDIS", "Тестирование обработки данных");
            ProcessResult(data);
            Logger.Log("Debug", "Тестирование CDDIS", "Данные успешно обработаны");
            Logger.Log("Debug", "Тестирование CDDIS", "Все тесты успешно пройдены");
            LocalWorker.DeleteTestFile(testFilePath, testFileName);
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Тестирование", $"Ошибка при тестировании \n{e.Message}");
            return;
        }
    }
    /// <summary>Метод получения данных из сети</summary>
    private static string GetResultFromNet(string username, string password, int defTriesToConnect)
    {
        try
        {
            WebWorker.LogInCDDIS(username, password, defTriesToConnect);
            Logger.Log("Debug", "Скачивание файла CDDIS", "Пользователь успешно авторизован");
            string result = WebWorker.DownloadDataCDDIS();
            Logger.Log("Debug", "Скачивание файла CDDIS", "Данные успешно скачаны");
            WebWorker.LogOutCDDIS();
            Logger.Log("Debug", "Скачивание файла CDDIS", "Успешно завершена сессия пользователя");
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>Метод обработки строки данных</summary>
    private static List<EopParam> ProcessResult(string data)
    {
        try
        {
            List<EopParam> result = DataProcesser.ProcessDataFromStringCDDIS(data);
            Logger.Log("Debug", "Обработка файла CDDIS", "Данные успешно обработаны");
            return result;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Обработка файла CDDIS", $"Ошибка при обработке файла\n {e.Message}");
            throw;
        }
    }
}