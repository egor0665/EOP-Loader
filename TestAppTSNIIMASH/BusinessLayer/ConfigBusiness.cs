using System;
using System.IO;
using System.Text.RegularExpressions;

class ConfigBusiness
{
    /// <summary>���� � �������������� ����������</summary>
    internal static ConfigData config { get; private set; }
    /// <summary>����� ��������� ���������� ��������� �� ����� config</summary>
    internal static int GetConfigsFromFile()
    {
        Logger.Log("Debug", "����������", "��������� ��������");
        if (!LocalWorker.CheckConfigExistance())
        {
            Logger.Log("Exception", "���������� �������", "���� config �� ������");
			throw new ConfigFileDoesNotExistException("�� ������� ����� ���� ������������");

		}
        try
        {
            config = LocalWorker.GetDataFromConfigFile();
            Logger.Log("Debug", "���������� �������", $"���� config ������� ������ \n-- ����� ������ �� ��������� - {config.Autorun}\n"
                + $"-- ��������� ������ � CDDIS - {config.AutorunCDDIS}\n"
                + $"-- ��������� ������ � IERS - {config.AutorunIERS}\n"
                + $"-- ������� ����������� - {config.CDDISDefTriesToConnect}\n"
                + $"-- ��� ������������ ��� ����������� �� ����� CDDIS - {config.CDDISWebsiteUsername}\n"
                + $"-- ������ ��� ����������� �� ����� CDDIS - {config.CDDISWebsitePassword}\n"
                + $"-- ���� ��� ���������� CDDIS- {config.CDDISSavePath}\n"
                + $"-- ��� ����� ��� ���������� CDDIS - {config.CDDISFileName}\n"
                + $"-- ���� ��� ���������� IERS- {config.IERSSavePath}\n"
                + $"-- ��� ����� ��� ���������� IERS - {config.IERSFileName}\n"
                + $"-- ���� � IERS ������ - {config.IERSLink}\n"
                );
            if (config.Autorun == true)
            {
                Logger.Log("Debug", "����������", "������ � ������ ������ �� ���������");
                return 1;
            }
            else
                return 2;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "���������� �������", $"������ ��� ������ ����� config\n {e.Message}");
            return 0;
        }
    }
	public struct ConfigData
	{
		/// <summary>�������� ����������� ��������� (���������)</summary>
		public bool Autorun;
		/// <summary>��� Autorun=true ��������� ������ � CDDIS</summary>
		public bool AutorunCDDIS;
		/// <summary>��� Autorun=true ��������� ������ � IERS</summary>
		public bool AutorunIERS;
		/// <summary>��� ������������ ��� ����������� �� ����� CDDIS</summary>
		public string CDDISWebsiteUsername;
		/// <summary>������ ��� ����������� �� ����� CDDIS</summary>
		public string CDDISWebsitePassword;
		/// <summary>���������� ������� ����� � ������ �������</summary>
		public int CDDISDefTriesToConnect;
		/// <summary>���� � ����� ����������</summary>
		public string CDDISSavePath;
		/// <summary>��� ����� ����������</summary>
		public string CDDISFileName;
		/// <summary>���� � ����� ����������</summary>
		public string IERSSavePath;
		/// <summary>��� ����� ����������</summary>
		public string IERSFileName;
		/// <summary>���� � IERS ������</summary>
		public string IERSLink;
		/// <summary>����������� ������������� � �������� ��������� ��������</summary>
		public ConfigData(string _Autorun, string _AutorunCDDIS, string _AutorunIERS, string _CDDISWebsiteUsername, string _CDDISWebsitePassword, string _CDDISDefTriesToConnect, string _CDDISSavePath, string _CDDISFileName, string _IERSSavePath, string _IERSFileName, string _IERSLink)
		{
			if (_Autorun == "true") Autorun = true;
			else if (_Autorun == "false") Autorun = false;
			else throw new IllegalConfigParamException("������ ��������� Autorun ");

			if (_AutorunCDDIS == "true") AutorunCDDIS = true;
			else if (_AutorunCDDIS == "false") AutorunCDDIS = false;
			else throw new IllegalConfigParamException("������ ��������� AutorunCDDIS ");

			if (_AutorunIERS == "true") AutorunIERS = true;
			else if (_AutorunIERS == "false") AutorunIERS = false;
			else throw new IllegalConfigParamException("������ ��������� AutorunIERS ");

			if (string.IsNullOrEmpty(_CDDISWebsiteUsername))
				throw new IllegalConfigParamException("������ ��������� CDDISWebsiteUsername");
			else CDDISWebsiteUsername = _CDDISWebsiteUsername;
			if (string.IsNullOrEmpty(_CDDISWebsitePassword))
				throw new IllegalConfigParamException("������ ��������� CDDISWebsitePassword");
			else CDDISWebsitePassword = _CDDISWebsitePassword;
			try
			{
				int dttc = Int16.Parse(_CDDISDefTriesToConnect);
				if (dttc < 1)
					throw new IllegalConfigParamException("������ ��������� CDDISDefTriesToConnect (�� ����� ���� ������ 1)");
				CDDISDefTriesToConnect = dttc;
			}
			catch (Exception)
			{
				throw new IllegalConfigParamException("������ ��������� CDDISDefTriesToConnect (�� �����)");
			}

			if (!Directory.Exists(_CDDISSavePath))
				throw new IllegalConfigParamException("������ ��������� CDDISSavePath (�� ������������ ����)");
			else CDDISSavePath = _CDDISSavePath;

			if (!Directory.Exists(_IERSSavePath))
				throw new IllegalConfigParamException("������ ��������� IERSSavePath (�� ������������ ����)");
			else IERSSavePath = _IERSSavePath;

			Regex regex = new Regex(@"^([a-zA-Z0-9_]+)\.(?!\.)([a-zA-Z0-9]{1,5})(?<!\.)$");
			if (!regex.Match(_CDDISFileName).Success)
			{
				throw new IllegalConfigParamException("������ ��������� CDDISFilePath (������ �������)");
			}
			else CDDISFileName = _CDDISFileName;

			if (!regex.Match(_IERSFileName).Success)
			{
				throw new IllegalConfigParamException("������ ��������� IERSFilePath (������ �������)");
			}
			else IERSFileName = _IERSFileName;

			regex = new Regex(@"^(https?:\/\/)?([\w-]{1,32}\.[\w-]{1,32})[^\s@]*$");
			if (!regex.Match(_IERSLink).Success)
			{
				throw new IllegalConfigParamException("������ ��������� IERSLink (������ �������)");
			}
			else IERSLink = _IERSLink;
		}
	}
}