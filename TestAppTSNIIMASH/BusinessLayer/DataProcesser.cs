using System;
using System.Collections.Generic;
using System.Globalization;

class DataProcesser
{
    /// <summary>Процедура обработки данных файла формата string CDDIS</summary>
    internal static List<EopParam> ProcessDataFromStringCDDIS(string data)
    {
        List<EopParam> tmpList = new();
        for (int i = 0; i < 3; i++)
        {
            int tmp = data.IndexOf('\n');
            data = data.Remove(0, tmp + 1);
        }
        string[] rows = data.Split(new char[] { '\n' });
        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i].Length > 2 && rows[i][0] != '*')
                tmpList.Add(EopParamFromStringCDDIS(rows[i]));
        }
        return tmpList;
    }
    /// <summary>Процедура обработки отдельной строки данных CDDIS</summary>
    private static EopParam EopParamFromStringCDDIS(string row)
    {
        int pred;
        if (row[0] == 'P')
            pred = 2;
        else pred = 1;
        row = row.Remove(0, 1);
        string[] data = row.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        EopParam tmpEopParam;
        tmpEopParam.DataNo = pred;
        tmpEopParam.T = new DateTime(2000 + Int16.Parse(data[0]), Int16.Parse(data[1]), Int16.Parse(data[2]), 0, 0, 0);
        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        tmpEopParam.Xp = double.Parse(data[4], formatter);
        tmpEopParam.ErrorX = double.Parse(data[5], formatter);
        tmpEopParam.Yp = double.Parse(data[6], formatter);
        tmpEopParam.ErrorY = double.Parse(data[7], formatter);
        tmpEopParam.DeltaUt1 = double.Parse(data[8], formatter);
        tmpEopParam.ErrorDeltaUt1 = double.Parse(data[9], formatter);
        return tmpEopParam;
    }
    /// <summary>Процедура обработки данных файла формата string IERS</summary>
    internal static List<EopParam> ProcessDataFromStringIERS(string data)
    {
        List<EopParam> tmpList = new();
        string[] rows = data.Split(new char[] { '\n' });
        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i] != "")
                tmpList.Add(EopParamFromStringIERS(rows[i]));
        }
        return tmpList;
    }
    /// <summary>Процедура обработки отдельной строки данных IERS</summary>
    private static EopParam EopParamFromStringIERS(string row)
    {
        int year = 0;
        if (row[0] == ' ') year = 0;
        else year = Int16.Parse(row[0].ToString());
        year = year * 10 + Int16.Parse(row[1].ToString());
        int month = 0;
        if (row[2] == ' ') month = 0;
        else month = Int16.Parse(row[2].ToString());
        month = month * 10 + Int16.Parse(row[3].ToString());
        int day = 0;
        if (row[4] == ' ') day = 0;
        else day = Int16.Parse(row[4].ToString());
        day = day * 10 + Int16.Parse(row[5].ToString());
        row = row.Remove(0, 6);
        string[] data = row.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int pred;
        if (data[1] == "I")
            pred = 1;
        else pred = 2;
        EopParam tmpEopParam;
        tmpEopParam.DataNo = pred;
        tmpEopParam.T = new DateTime(2000 + year, month, day, 0, 0, 0);
        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        tmpEopParam.Xp = double.Parse(data[2], formatter);
        tmpEopParam.ErrorX = double.Parse(data[3], formatter);
        tmpEopParam.Yp = double.Parse(data[4], formatter);
        tmpEopParam.ErrorY = double.Parse(data[5], formatter);
        if (data[6].Length == 1)
        {
            tmpEopParam.DeltaUt1 = double.Parse(data[7], formatter);
            tmpEopParam.ErrorDeltaUt1 = double.Parse(data[8], formatter);
        }
        else
        {
            data[6] = data[6].Remove(0, 1);
            tmpEopParam.DeltaUt1 = double.Parse(data[6], formatter);
            tmpEopParam.ErrorDeltaUt1 = double.Parse(data[7], formatter);
        }
        return tmpEopParam;
    }
}