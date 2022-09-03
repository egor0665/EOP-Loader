using System;
using System.IO;
using System.Text.RegularExpressions;

class ConfigBusiness
{
    /// <summary>Поле с конфигурациями приложения</summary>
    internal static ConfigData config { get; private set; }
    /// <summary>Метод получения параметров программы из файла config</summary>
    internal static int GetConfigsFromFile()
    {
        Logger.Log("Debug", "Выполнение", "Программа запущена");
        if (!LocalWorker.CheckConfigExistance())
        {
            Logger.Log("Exception", "Считывание конфига", "Файл config не найден");
			throw new ConfigFileDoesNotExistException("Не удалось найти файл конфигурации");

		}
        try
        {
            config = LocalWorker.GetDataFromConfigFile();
            Logger.Log("Debug", "Считывание конфига", $"Файл config успешно считан \n-- Режим работы по умолчанию - {config.Autorun}\n"
                + $"-- скачивать данные с CDDIS - {config.AutorunCDDIS}\n"
                + $"-- скачивать данные с IERS - {config.AutorunIERS}\n"
                + $"-- Попыток подключения - {config.CDDISDefTriesToConnect}\n"
                + $"-- Имя пользователя для авторизации на сайте CDDIS - {config.CDDISWebsiteUsername}\n"
                + $"-- Пароль для авторизации на сайте CDDIS - {config.CDDISWebsitePassword}\n"
                + $"-- Путь для сохранения CDDIS- {config.CDDISSavePath}\n"
                + $"-- Имя файла для сохранения CDDIS - {config.CDDISFileName}\n"
                + $"-- Путь для сохранения IERS- {config.IERSSavePath}\n"
                + $"-- Имя файла для сохранения IERS - {config.IERSFileName}\n"
                + $"-- Путь к IERS данным - {config.IERSLink}\n"
                );
            if (config.Autorun == true)
            {
                Logger.Log("Debug", "Выполнение", "Запуск в режиме работы по умолчанию");
                return 1;
            }
            else
                return 2;
        }
        catch (Exception e)
        {
            Logger.Log("Fatal", "Считывание конфига", $"Ошибка при чтении файла config\n {e.Message}");
            return 0;
        }
    }
	public struct ConfigData
	{
		/// <summary>Параметр автозапуска программы (Интерфейс)</summary>
		public bool Autorun;
		/// <summary>При Autorun=true скачивать данные с CDDIS</summary>
		public bool AutorunCDDIS;
		/// <summary>При Autorun=true скачивать данные с IERS</summary>
		public bool AutorunIERS;
		/// <summary>Имя пользователя для авторизации на сайте CDDIS</summary>
		public string CDDISWebsiteUsername;
		/// <summary>Пароль для авторизации на сайте CDDIS</summary>
		public string CDDISWebsitePassword;
		/// <summary>Количество попыток входа в личный кабинет</summary>
		public int CDDISDefTriesToConnect;
		/// <summary>Путь к файлу сохранения</summary>
		public string CDDISSavePath;
		/// <summary>Имя файла сохранения</summary>
		public string CDDISFileName;
		/// <summary>Путь к файлу сохранения</summary>
		public string IERSSavePath;
		/// <summary>Имя файла сохранения</summary>
		public string IERSFileName;
		/// <summary>Путь к IERS данным</summary>
		public string IERSLink;
		/// <summary>Конструктор инициализации и проверки введенных значений</summary>
		public ConfigData(string _Autorun, string _AutorunCDDIS, string _AutorunIERS, string _CDDISWebsiteUsername, string _CDDISWebsitePassword, string _CDDISDefTriesToConnect, string _CDDISSavePath, string _CDDISFileName, string _IERSSavePath, string _IERSFileName, string _IERSLink)
		{
			if (_Autorun == "true") Autorun = true;
			else if (_Autorun == "false") Autorun = false;
			else throw new IllegalConfigParamException("Ошибка аттрибута Autorun ");

			if (_AutorunCDDIS == "true") AutorunCDDIS = true;
			else if (_AutorunCDDIS == "false") AutorunCDDIS = false;
			else throw new IllegalConfigParamException("Ошибка аттрибута AutorunCDDIS ");

			if (_AutorunIERS == "true") AutorunIERS = true;
			else if (_AutorunIERS == "false") AutorunIERS = false;
			else throw new IllegalConfigParamException("Ошибка аттрибута AutorunIERS ");

			if (string.IsNullOrEmpty(_CDDISWebsiteUsername))
				throw new IllegalConfigParamException("Ошибка аттрибута CDDISWebsiteUsername");
			else CDDISWebsiteUsername = _CDDISWebsiteUsername;
			if (string.IsNullOrEmpty(_CDDISWebsitePassword))
				throw new IllegalConfigParamException("Ошибка аттрибута CDDISWebsitePassword");
			else CDDISWebsitePassword = _CDDISWebsitePassword;
			try
			{
				int dttc = Int16.Parse(_CDDISDefTriesToConnect);
				if (dttc < 1)
					throw new IllegalConfigParamException("Ошибка аттрибута CDDISDefTriesToConnect (Не может быть меньше 1)");
				CDDISDefTriesToConnect = dttc;
			}
			catch (Exception)
			{
				throw new IllegalConfigParamException("Ошибка аттрибута CDDISDefTriesToConnect (Не число)");
			}

			if (!Directory.Exists(_CDDISSavePath))
				throw new IllegalConfigParamException("Ошибка аттрибута CDDISSavePath (Не существующий путь)");
			else CDDISSavePath = _CDDISSavePath;

			if (!Directory.Exists(_IERSSavePath))
				throw new IllegalConfigParamException("Ошибка аттрибута IERSSavePath (Не существующий путь)");
			else IERSSavePath = _IERSSavePath;

			Regex regex = new Regex(@"^([a-zA-Z0-9_]+)\.(?!\.)([a-zA-Z0-9]{1,5})(?<!\.)$");
			if (!regex.Match(_CDDISFileName).Success)
			{
				throw new IllegalConfigParamException("Ошибка аттрибута CDDISFilePath (Ошибка формата)");
			}
			else CDDISFileName = _CDDISFileName;

			if (!regex.Match(_IERSFileName).Success)
			{
				throw new IllegalConfigParamException("Ошибка аттрибута IERSFilePath (Ошибка формата)");
			}
			else IERSFileName = _IERSFileName;

			regex = new Regex(@"^(https?:\/\/)?([\w-]{1,32}\.[\w-]{1,32})[^\s@]*$");
			if (!regex.Match(_IERSLink).Success)
			{
				throw new IllegalConfigParamException("Ошибка аттрибута IERSLink (Ошибка формата)");
			}
			else IERSLink = _IERSLink;
		}
	}
}