using System;
/// <summary>���������� ������������ ��� ������������� ��������� ������ �� �������� �����������</summary>
class NoAuthentityTokenException : Exception
{
    public NoAuthentityTokenException(string message)
        : base(message) { }
}
/// <summary>���������� ������������ ��� ������������� ��������������</summary>
class CanNotLogInException : Exception
{
    public CanNotLogInException(string message)
        : base(message) { }
}
/// <summary>���������� ������������ ��� ������������� ���������� ����� ������ �� ����</summary>
class CanNotDownloadDataException : Exception
{
    public CanNotDownloadDataException(string message)
        : base(message) { }
}
/// <summary>���������� ������������ ��� ���������� ����������������� �����</summary>
class ConfigFileDoesNotExistException : Exception
{
    public ConfigFileDoesNotExistException(string message)
        : base(message) { }
}
/// <summary>���������� ������������ ��� ������� ������������ ���������� � ����� ������������</summary>
class IllegalConfigParamException : Exception
{
    public IllegalConfigParamException(string message)
        : base(message) { }
}