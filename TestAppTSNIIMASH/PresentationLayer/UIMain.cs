using System;

namespace TestAppTSNIIMASH
{
    class UIMain
    {
        static void Main(string[] args)
        {
            StartCommandListener();
            Logger.Log("Debug", "����������", "��������� ��������� ������");
        }
        private static void StartCommandListener()
        {
            string Input = "";
            int confres;
            try
            {
                confres = ConfigBusiness.GetConfigsFromFile();
            }
            catch (Exception e)
            {
                Logger.Log("Fatal", "���������� �������", $"������ ��� ������ ����� config\n {e.Message}");
                return;
            }
            if (confres == 1)
            {
                Input = "0";
                BusinessMain.DownloadDataAutorun();
            }
            else if (confres == 0)
            {
                Input = "0";
            }
            while (Input != "0")
            {
                ShowMenu();
                Input = Console.ReadLine();
                BusinessMain.WorkInput(Input);
            }
        }
        /// <summary>����� ����</summary>
        private static void ShowMenu()
        {
            Console.WriteLine("1 - ������� ��� ������");
            Console.WriteLine("2 - ������� ������ CDDIS");
            Console.WriteLine("3 - ������� � ���������� ������ CDDIS");
            Console.WriteLine("4 - ���������� ������ CDDIS");
            Console.WriteLine("5 - ������� ������ IERS");
            Console.WriteLine("6 - ������� � ���������� ������ IERS");
            Console.WriteLine("7 - ���������� ������ IERS");
            Console.WriteLine("8 - ����������� ��� �������");
            Console.WriteLine("0 - �����");
        }
    }
}