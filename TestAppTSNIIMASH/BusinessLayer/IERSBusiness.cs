using System;
using System.Collections.Generic;
using System.IO;

class IERSBusiness
{
    /// <summary>Метод скачивания данных из сети</summary>
    internal static void DownloadData(string savePath, string fileName, string link)
    {
        Logger.Log("Debug", "Скачивание файла IERS", "Скачивание данных запущено");
        WebWorker.CreateVariables();
        try
        {
            string data = GetResultFromNet(link);
            LocalWorker.SaveData(data, savePath, fileName);
            Logger.Log("Debug", "Скачивание файла IERS", $"Данные успешно сохранены по пути {savePath}\\{fileName}");
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Скачивание файла IERS", $"Ошибка при скачивании файла\n {e.Message}");
            throw;
        }
    }
    /// <summary>Метод скачивания данных из сети и их разбора</summary>
    internal static List<EopParam> DownloadAndRefactorData(string link)
    {
        Logger.Log("Debug", "Скачивание и обработка IERS", "Скачивание и обработка данных запущена");
        WebWorker.CreateVariables();
        try
        {
            string data = GetResultFromNet(link);
            List<EopParam> EopParams = ProcessResult(data);
            Logger.Log("Debug", "Скачивание и обработка IERS", "Данные успешно скачаны и обработаны");
            return EopParams;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Скачивание и обработка IERS", $"Ошибка при скачивании и обработке \n{e.Message}");
            throw;
        }
    }
    /// <summary>Метод разбора данных из файла</summary>
    internal static List<EopParam> RefactorData(string savePath, string fileName)
    {
        Logger.Log("Debug", "Обработка файла IERS", "Обработка файла запущена");
        try
        {
            string data = LocalWorker.ReadData(savePath, fileName);
            Logger.Log("Debug", "Обработка файла IERS", $"Данные успешно считаны из файла {savePath}\\{fileName}");
            List<EopParam> EopParams = ProcessResult(data);
            return EopParams;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Обработка файла IERS", $"Ошибка при обработке файла\n {e.Message}");
            throw;
        }
    }
    /// <summary>Метод тестирования</summary>
    internal static void Test(string link)
    {
        Logger.Log("Debug", "Тестирование IERS", "Тестирование запущено");
        WebWorker.CreateVariables();
        try
        {
            Logger.Log("Debug", "Тестирование IERS", "Тестирование доступа к данным в сети");
            string data = GetResultFromNet(link);
            Logger.Log("Debug", "Тестирование IERS", "Данные успешно получены из сети");
            Logger.Log("Debug", "Тестирование IERS", "Тестирование сохранения данных");
            string testFilePath = Directory.GetCurrentDirectory();
            string testFileName = "test.txt";
            LocalWorker.SaveData(data, testFilePath, testFileName);
            Logger.Log("Debug", "Тестирование IERS", "Данные успешно сохранены в файл");
            Logger.Log("Debug", "Тестирование IERS", "Тестирование получение данных из файла");
            data = LocalWorker.ReadData(testFilePath, testFileName);
            Logger.Log("Debug", "Тестирование IERS", $"Данные успешно считаны из файла");
            Logger.Log("Debug", "Тестирование IERS", "Тестирование обработки данных");
            ProcessResult(data);
            Logger.Log("Debug", "Тестирование IERS", "Данные успешно обработаны");
            Logger.Log("Debug", "Тестирование IERS", "Все тесты успешно пройдены");
            LocalWorker.DeleteTestFile(testFilePath, testFileName);
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Тестирование", $"Ошибка при тестировании \n{e.Message}");
            return;
        }
    }
    /// <summary>Метод получения данных из сети</summary>
    private static string GetResultFromNet(string link)
    {
        try
        {
            return WebWorker.DownloadDataIERS(link);
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>Метод обработки строки данных</summary>
    internal static List<EopParam> ProcessResult(string data)
    {
        try
        {
            List<EopParam> result = DataProcesser.ProcessDataFromStringIERS(data);
            Logger.Log("Debug", "Обработка файла IERS", "Данные успешно обработаны");
            return result;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Обработка файла IERS", $"Ошибка при обработке файла\n {e.Message}");
            throw;
        }
    }

}