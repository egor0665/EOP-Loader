using System;
/// <summary>Исключение происходящее при невозможности получения токена со страницы авторизации</summary>
class NoAuthentityTokenException : Exception
{
    public NoAuthentityTokenException(string message)
        : base(message) { }
}
/// <summary>Исключение происходящее при невозможности аутентификации</summary>
class CanNotLogInException : Exception
{
    public CanNotLogInException(string message)
        : base(message) { }
}
/// <summary>Исключение происходящее при невозможности скачивания файла данных из сети</summary>
class CanNotDownloadDataException : Exception
{
    public CanNotDownloadDataException(string message)
        : base(message) { }
}
/// <summary>Исключение происходящее при отсутствии конфигурационного файла</summary>
class ConfigFileDoesNotExistException : Exception
{
    public ConfigFileDoesNotExistException(string message)
        : base(message) { }
}
/// <summary>Исключение происходящее при наличии некорректных параметров в файле конфигурации</summary>
class IllegalConfigParamException : Exception
{
    public IllegalConfigParamException(string message)
        : base(message) { }
}