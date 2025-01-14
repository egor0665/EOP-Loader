using System;
using System.Configuration;
using System.IO;
using static ConfigBusiness;

class LocalWorker
{
	/// <summary>����� �������� ������������� config �����</summary>
	internal static bool CheckConfigExistance()
	{
		return File.Exists(Directory.GetCurrentDirectory() + "\\" + "App.config") || File.Exists(Directory.GetCurrentDirectory() + "\\" + "TestAppTSNIIMASH.dll.config");
	}
	/// <summary>����� ���������� ����� � ����</summary>
	internal static void LogInFile(string logMessage)
	{
		try
		{
			File.AppendAllText(Directory.GetCurrentDirectory() + "\\" + "Program1.log", "\n" + logMessage);
		}
		catch (Exception)
		{
			throw;
		}
	}

	/// <summary>����� ���������� config �����</summary>
	internal static ConfigData GetDataFromConfigFile()
	{
		return new ConfigData(ConfigurationManager.AppSettings.Get("Autorun"),
			ConfigurationManager.AppSettings.Get("AutorunCDDIS"),
			ConfigurationManager.AppSettings.Get("AutorunIERS"),
			ConfigurationManager.AppSettings.Get("CDDISWebsiteUsername"),
			ConfigurationManager.AppSettings.Get("CDDISWebsitePassword"),
			ConfigurationManager.AppSettings.Get("CDDISDefTriesToConnect"),
			ConfigurationManager.AppSettings.Get("CDDISSavePath"),
			ConfigurationManager.AppSettings.Get("CDDISFileName"),
			ConfigurationManager.AppSettings.Get("IERSSavePath"),
			ConfigurationManager.AppSettings.Get("IERSFileName"),
			ConfigurationManager.AppSettings.Get("IERSLink")
			);
	}

	/// <summary>����� ���������� ������ �� ����� � ������� � �����</summary>
	internal static string ReadData(object filePath, string fileName)
	{
		try
		{
			StreamReader sr = new StreamReader(filePath + "\\" + fileName);
			string line = sr.ReadLine();
			string result = line;
			while (line != null)
			{
				line = sr.ReadLine();
				result += line + "\n";
			}
			sr.Close();
			return result;
		}
		catch (Exception)
		{
			throw;
		}
	}

	/// <summary>����� ���������� ����� � ����� � ����</summary>
	internal static void SaveData(string result, string savePath, string fileName)
	{
		try
		{
			File.WriteAllText(savePath + "\\" + fileName, result);
		}
		catch (Exception)
		{
			throw;
		}
	}

	/// <summary>����� �������� ��������� �����</summary>
	internal static void DeleteTestFile(string testFilePath, string testFileName)
	{
		File.Delete(testFilePath + "\\" + testFileName);
	}
}