using System.Net;
using System.Text.RegularExpressions;

class WebWorker
{
	/// <summary>Хранилище cookie файлов</summary>
	private static CookieContainer cookieJar;
	/// <summary>Веб клиент с куки файлами</summary>
	private static CookieAwareWebClient client;
	/// <summary>Метод создания Веб переменных</summary>
	internal static void CreateVariables()
	{
		cookieJar = new CookieContainer();
		client = new CookieAwareWebClient(cookieJar);
	}
	/// <summary>Метод авторизации на сайте</summary>
	internal static void LogInCDDIS(string username, string password, int defTriesToConnect)
	{
		client.Method = "POST";
		bool logInCheck = false;
		int TryNum = 0;
		while (!logInCheck && TryNum < defTriesToConnect)
		{
			string token = GetAuthencityToken();
			string postData =
				string.Format("utf8=%E2%9C%93&authenticity_token={0}&username={1}&password={2}&client_id=&redirect_uri=&commit=Log+in", token, username, password);
			string response = client.UploadString("https://urs.earthdata.nasa.gov/login", postData);
			logInCheck = response.Contains("Profile Information") && response.Contains(username);
			TryNum++;
		}
		if (TryNum == defTriesToConnect && !logInCheck)
			throw new CanNotLogInException("Не удалось войти в личный кабинет");
	}
	/// <summary>Метод выхода из аккаунта</summary>
	internal static void LogOutCDDIS()
	{
		client.DownloadString("https://urs.earthdata.nasa.gov/logout");
	}
	/// <summary>Методполучения токена аутентификации со страницы авторизации</summary>
	internal static string GetAuthencityToken()
	{
		string response = client.DownloadString("https://urs.earthdata.nasa.gov/home");
		string token = Regex.Match(response, "authenticity_token\" value=\"(.+?)\"").Groups[1].Value;
		if (token == "")
			throw new NoAuthentityTokenException("Не удалось получить токен аутентификации");
		return token;
	}
	/// <summary>Метод скачивания файла данных о параметрах Земли CDDIS</summary>
	internal static string DownloadDataCDDIS()
	{
		client.Method = "GET";
		string response = client.DownloadString("https://cddis.nasa.gov/archive/products/iers/mark3.out");
		string redirectURL = Regex.Match(response, "var redirectURL = \"(.+?)\"").Groups[1].Value;
		if (redirectURL != "")
		{
			client.DownloadString(redirectURL);
			string data = client.DownloadString("https://cddis.nasa.gov/archive/products/iers/mark3.out");
			return data;
		}
		else
			throw new CanNotDownloadDataException("Не удалось скачать файл");

	}
	/// <summary>Метод скачивания файла данных о параметрах Земли IERS</summary>
	internal static string DownloadDataIERS(string link)
	{
		client.Method = "GET";
		string data = client.DownloadString(link);
		if (data.Contains("Not Found"))
		{
			throw new CanNotDownloadDataException("Не удалось скачать файл IERS (Файл по ссылке не найден)");
		}
		else
			return data;
	}
}