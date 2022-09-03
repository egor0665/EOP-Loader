using System;
using System.Collections.Generic;
using System.IO;

class BusinessMain
{
    /// <summary>Таблица параметров вращения Земли</summary>
    public static List<EopParam> EopParams { get; set; }
    /// <summary>Обработка команд пользователя</summary>
    internal static void WorkInput(string input)
    {
        /*
        1 - Скачать ВСЕ данные
        2 - Скачать данные CDDIS
        3 - Скачать и обработать данные CDDIS
        4 - Обработать данные CDDIS
        5 - Скачать данные IERS
        6 - Скачать и обработать данные IERS
        7 - Обработать данные IERS
        8 - Тестировать все функции
        0 - Выйти
        */
        try
        {
            switch (input)
            {
                case "1":
                    DownloadDataAll();
                    break;
                case "2":
                    CDDISBusiness.DownloadData(
                        ConfigBusiness.config.CDDISSavePath,
                        ConfigBusiness.config.CDDISFileName,
                        ConfigBusiness.config.CDDISWebsiteUsername,
                        ConfigBusiness.config.CDDISWebsitePassword,
                        ConfigBusiness.config.CDDISDefTriesToConnect
                        );
                    break;
                case "3":
                    EopParams = CDDISBusiness.DownloadAndRefactorData(
                        ConfigBusiness.config.CDDISWebsiteUsername,
                        ConfigBusiness.config.CDDISWebsitePassword,
                        ConfigBusiness.config.CDDISDefTriesToConnect);
                    TestEopParamsOutput();
                    break;
                case "4":
                    EopParams = CDDISBusiness.RefactorData(
                        ConfigBusiness.config.CDDISSavePath,
                        ConfigBusiness.config.CDDISFileName
                        );
                    TestEopParamsOutput();
                    break;
                case "5":
                    IERSBusiness.DownloadData(
                        ConfigBusiness.config.IERSSavePath,
                        ConfigBusiness.config.IERSFileName,
                        ConfigBusiness.config.IERSLink
                        );
                    break;
                case "6":
                    EopParams = IERSBusiness.DownloadAndRefactorData(
                        ConfigBusiness.config.IERSLink
                        );
                    TestEopParamsOutput();
                    break;
                case "7":
                    EopParams = IERSBusiness.RefactorData(
                        ConfigBusiness.config.IERSSavePath,
                        ConfigBusiness.config.IERSFileName
                        );
                    TestEopParamsOutput();
                    break;
                case "8":
                    CDDISBusiness.Test(
                        ConfigBusiness.config.CDDISWebsiteUsername,
                        ConfigBusiness.config.CDDISWebsitePassword,
                        ConfigBusiness.config.CDDISDefTriesToConnect
                        );
                    IERSBusiness.Test(
                        ConfigBusiness.config.IERSLink
                        );
                    break;
                default:
                    break;
            }
        }
        catch(Exception e)
        {
            Logger.Log("Fatal", "Тестирование", $"\n {e.Message}");
            return;
        }
    }
    /// <summary>Обработка автозапуска</summary>
    internal static void DownloadDataAutorun()
    {
        if (ConfigBusiness.config.AutorunCDDIS == true && ConfigBusiness.config.AutorunIERS == true)
            DownloadDataAll();
        else if (ConfigBusiness.config.AutorunCDDIS == true)
            CDDISBusiness.DownloadData(
                ConfigBusiness.config.CDDISSavePath,
                ConfigBusiness.config.CDDISFileName,
                ConfigBusiness.config.CDDISWebsiteUsername,
                ConfigBusiness.config.CDDISWebsitePassword,
                ConfigBusiness.config.CDDISDefTriesToConnect
                );
        else if (ConfigBusiness.config.AutorunIERS == true)
            IERSBusiness.DownloadData(
                 ConfigBusiness.config.IERSSavePath,
                 ConfigBusiness.config.IERSFileName,
                 ConfigBusiness.config.IERSLink
                 );
    }

    /// <summary>Скачивание данных из обоих источников</summary>
    internal static void DownloadDataAll()
    {
        Logger.Log("Debug", "Скачивание файлов", "Скачивание данных запущено");
        CDDISBusiness.DownloadData(
            ConfigBusiness.config.CDDISSavePath,
            ConfigBusiness.config.CDDISFileName,
            ConfigBusiness.config.CDDISWebsiteUsername,
            ConfigBusiness.config.CDDISWebsitePassword,
            ConfigBusiness.config.CDDISDefTriesToConnect
            );
        IERSBusiness.DownloadData(
            ConfigBusiness.config.IERSSavePath,
            ConfigBusiness.config.IERSFileName,
            ConfigBusiness.config.IERSLink
            );
    }
    /// <summary>Вывод обработанных данных</summary>
    private static void TestEopParamsOutput()
    {
        foreach (EopParam i in EopParams)
        {
            Console.WriteLine($"{i.DataNo} {i.T} {i.Xp} {i.ErrorX} {i.Yp} {i.ErrorY} {i.DeltaUt1} {i.ErrorDeltaUt1}");
        }
    }
}