using System;
using System.Collections.Generic;
using System.IO;

class IERSBusiness
{
    /// <summary>����� ���������� ������ �� ����</summary>
    internal static void DownloadData(string savePath, string fileName, string link)
    {
        Logger.Log("Debug", "���������� ����� IERS", "���������� ������ ��������");
        WebWorker.CreateVariables();
        try
        {
            string data = GetResultFromNet(link);
            LocalWorker.SaveData(data, savePath, fileName);
            Logger.Log("Debug", "���������� ����� IERS", $"������ ������� ��������� �� ���� {savePath}\\{fileName}");
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "���������� ����� IERS", $"������ ��� ���������� �����\n {e.Message}");
            throw;
        }
    }
    /// <summary>����� ���������� ������ �� ���� � �� �������</summary>
    internal static List<EopParam> DownloadAndRefactorData(string link)
    {
        Logger.Log("Debug", "���������� � ��������� IERS", "���������� � ��������� ������ ��������");
        WebWorker.CreateVariables();
        try
        {
            string data = GetResultFromNet(link);
            List<EopParam> EopParams = ProcessResult(data);
            Logger.Log("Debug", "���������� � ��������� IERS", "������ ������� ������� � ����������");
            return EopParams;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "���������� � ��������� IERS", $"������ ��� ���������� � ��������� \n{e.Message}");
            throw;
        }
    }
    /// <summary>����� ������� ������ �� �����</summary>
    internal static List<EopParam> RefactorData(string savePath, string fileName)
    {
        Logger.Log("Debug", "��������� ����� IERS", "��������� ����� ��������");
        try
        {
            string data = LocalWorker.ReadData(savePath, fileName);
            Logger.Log("Debug", "��������� ����� IERS", $"������ ������� ������� �� ����� {savePath}\\{fileName}");
            List<EopParam> EopParams = ProcessResult(data);
            return EopParams;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "��������� ����� IERS", $"������ ��� ��������� �����\n {e.Message}");
            throw;
        }
    }
    /// <summary>����� ������������</summary>
    internal static void Test(string link)
    {
        Logger.Log("Debug", "������������ IERS", "������������ ��������");
        WebWorker.CreateVariables();
        try
        {
            Logger.Log("Debug", "������������ IERS", "������������ ������� � ������ � ����");
            string data = GetResultFromNet(link);
            Logger.Log("Debug", "������������ IERS", "������ ������� �������� �� ����");
            Logger.Log("Debug", "������������ IERS", "������������ ���������� ������");
            string testFilePath = Directory.GetCurrentDirectory();
            string testFileName = "test.txt";
            LocalWorker.SaveData(data, testFilePath, testFileName);
            Logger.Log("Debug", "������������ IERS", "������ ������� ��������� � ����");
            Logger.Log("Debug", "������������ IERS", "������������ ��������� ������ �� �����");
            data = LocalWorker.ReadData(testFilePath, testFileName);
            Logger.Log("Debug", "������������ IERS", $"������ ������� ������� �� �����");
            Logger.Log("Debug", "������������ IERS", "������������ ��������� ������");
            ProcessResult(data);
            Logger.Log("Debug", "������������ IERS", "������ ������� ����������");
            Logger.Log("Debug", "������������ IERS", "��� ����� ������� ��������");
            LocalWorker.DeleteTestFile(testFilePath, testFileName);
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "������������", $"������ ��� ������������ \n{e.Message}");
            return;
        }
    }
    /// <summary>����� ��������� ������ �� ����</summary>
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
    /// <summary>����� ��������� ������ ������</summary>
    internal static List<EopParam> ProcessResult(string data)
    {
        try
        {
            List<EopParam> result = DataProcesser.ProcessDataFromStringIERS(data);
            Logger.Log("Debug", "��������� ����� IERS", "������ ������� ����������");
            return result;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "��������� ����� IERS", $"������ ��� ��������� �����\n {e.Message}");
            throw;
        }
    }

}