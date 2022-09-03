using System;
using System.Collections.Generic;
using System.IO;

class BusinessMain
{
    /// <summary>������� ���������� �������� �����</summary>
    public static List<EopParam> EopParams { get; set; }
    /// <summary>��������� ������ ������������</summary>
    internal static void WorkInput(string input)
    {
        /*
        1 - ������� ��� ������
        2 - ������� ������ CDDIS
        3 - ������� � ���������� ������ CDDIS
        4 - ���������� ������ CDDIS
        5 - ������� ������ IERS
        6 - ������� � ���������� ������ IERS
        7 - ���������� ������ IERS
        8 - ����������� ��� �������
        0 - �����
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
            Logger.Log("Fatal", "������������", $"\n {e.Message}");
            return;
        }
    }
    /// <summary>��������� �����������</summary>
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

    /// <summary>���������� ������ �� ����� ����������</summary>
    internal static void DownloadDataAll()
    {
        Logger.Log("Debug", "���������� ������", "���������� ������ ��������");
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
    /// <summary>����� ������������ ������</summary>
    private static void TestEopParamsOutput()
    {
        foreach (EopParam i in EopParams)
        {
            Console.WriteLine($"{i.DataNo} {i.T} {i.Xp} {i.ErrorX} {i.Yp} {i.ErrorY} {i.DeltaUt1} {i.ErrorDeltaUt1}");
        }
    }
}