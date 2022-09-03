using System;
using System.Collections.Generic;
using System.IO;

class CDDISBusiness
{
    /// <summary>����� ���������� ������ �� ����</summary>
    internal static void DownloadData(string savePath, string fileName, string username, string password, int defTriesToConnect)
    {
        Logger.Log("Debug", "���������� ����� CDDIS", "���������� ������ ��������");
        WebWorker.CreateVariables();
        try
        {
            string data = GetResultFromNet(username, password, defTriesToConnect);
            LocalWorker.SaveData(data, savePath, fileName);
            Logger.Log("Debug", "���������� ����� CDDIS", $"������ ������� ��������� �� ���� {savePath}\\{fileName}");
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "���������� ����� CDDIS", $"������ ��� ���������� �����\n {e.Message}");
            throw;
        }
    }
    /// <summary>����� ���������� ������ �� ���� � �� �������</summary>
    internal static List<EopParam> DownloadAndRefactorData(string username, string password, int defTriesToConnect)
    {
        Logger.Log("Debug", "���������� � ��������� CDDIS", "���������� � ��������� ������ ��������");
        WebWorker.CreateVariables();
        try
        {
            string data = GetResultFromNet(username, password, defTriesToConnect);
            List<EopParam> EopParams = ProcessResult(data);
            Logger.Log("Debug", "���������� � ��������� CDDIS", "������ ������� ������� � ����������");
            return EopParams;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "���������� � ��������� CDDIS", $"������ ��� ���������� � ��������� \n{e.Message}");
            throw;
        }
    }
    /// <summary>����� ������� ������ �� �����</summary>
    internal static List<EopParam> RefactorData(string savePath, string fileName)
    {
        Logger.Log("Debug", "��������� ����� CDDIS", "��������� ����� ��������");
        try
        {
            string data = LocalWorker.ReadData(savePath, fileName);
            Logger.Log("Debug", "��������� ����� CDDIS", $"������ ������� ������� �� ����� {savePath}\\{fileName}");
            List<EopParam> EopParams = ProcessResult(data);
            return EopParams;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "��������� ����� CDDIS", $"������ ��� ��������� �����\n {e.Message}");
            throw;
        }
    }
    /// <summary>����� ������������</summary>
    internal static void Test(string username, string password, int defTriesToConnect)
    {
        Logger.Log("Debug", "������������ CDDIS", "������������ ��������");
        WebWorker.CreateVariables();
        try
        {
            Logger.Log("Debug", "������������ CDDIS", "������������ ������� � ������ � ����");
            string data = GetResultFromNet(username, password, defTriesToConnect);
            Logger.Log("Debug", "������������ CDDIS", "������ ������� �������� �� ����");
            Logger.Log("Debug", "������������ CDDIS", "������������ ���������� ������");
            string testFilePath = Directory.GetCurrentDirectory();
            string testFileName = "test.txt";
            LocalWorker.SaveData(data, testFilePath, testFileName);
            Logger.Log("Debug", "������������ CDDIS", "������ ������� ��������� � ����");
            Logger.Log("Debug", "������������ CDDIS", "������������ ��������� ������ �� �����");
            data = LocalWorker.ReadData(testFilePath, testFileName);
            Logger.Log("Debug", "������������ CDDIS", $"������ ������� ������� �� �����");
            Logger.Log("Debug", "������������ CDDIS", "������������ ��������� ������");
            ProcessResult(data);
            Logger.Log("Debug", "������������ CDDIS", "������ ������� ����������");
            Logger.Log("Debug", "������������ CDDIS", "��� ����� ������� ��������");
            LocalWorker.DeleteTestFile(testFilePath, testFileName);
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "������������", $"������ ��� ������������ \n{e.Message}");
            return;
        }
    }
    /// <summary>����� ��������� ������ �� ����</summary>
    private static string GetResultFromNet(string username, string password, int defTriesToConnect)
    {
        try
        {
            WebWorker.LogInCDDIS(username, password, defTriesToConnect);
            Logger.Log("Debug", "���������� ����� CDDIS", "������������ ������� �����������");
            string result = WebWorker.DownloadDataCDDIS();
            Logger.Log("Debug", "���������� ����� CDDIS", "������ ������� �������");
            WebWorker.LogOutCDDIS();
            Logger.Log("Debug", "���������� ����� CDDIS", "������� ��������� ������ ������������");
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>����� ��������� ������ ������</summary>
    private static List<EopParam> ProcessResult(string data)
    {
        try
        {
            List<EopParam> result = DataProcesser.ProcessDataFromStringCDDIS(data);
            Logger.Log("Debug", "��������� ����� CDDIS", "������ ������� ����������");
            return result;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "��������� ����� CDDIS", $"������ ��� ��������� �����\n {e.Message}");
            throw;
        }
    }
}