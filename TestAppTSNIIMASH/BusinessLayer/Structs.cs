using System;
/// <summary> Структура для хранения параметров вращения Земли</summary>
public struct EopParam
{
	/// <summary>Признак информации (1 - реальное значение, 2 - прогнозируемое)</summary>
	public double DataNo;
	/// <summary>Время принажлежности данных (обычно = 0h суток) по шкале UTC</summary>
	public DateTime T;
	/// <summary>Координата x мгновенного полюса на 0h суток по шкале UTС [м]</summary>
	public double Xp;
	/// <summary>Погрешность координаты x мгновенного полюса на 0h суток по шкале UTС [м]</summary>
	public double ErrorX;
	/// <summary>Координата y мгновенного полюса на 0h суток по шкале UTС [м]</summary>
	public double Yp;
	/// <summary>Погрешность координаты y мгновенного полюса на 0h суток по шкале UTС [м]</summary>
	public double ErrorY;
	/// <summary>Значение поправки deltaUT1 на 0h суток по шкале UTС [с]</summary>
	public double DeltaUt1;
	/// <summary>Погрешность значения поправки deltaUT1 на 0h суток по шкале UTС [с]</summary>
	public double ErrorDeltaUt1;

}