using System;

namespace TestAppTSNIIMASH
{
    class UIMain
    {
        static void Main(string[] args)
        {
            StartCommandListener();
            Logger.Log("Debug", "Выполнение", "Программа завершила работу");
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
                Logger.Log("Fatal", "Считывание конфига", $"Ошибка при чтении файла config\n {e.Message}");
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
        /// <summary>Вывод меню</summary>
        private static void ShowMenu()
        {
            Console.WriteLine("1 - Скачать ВСЕ данные");
            Console.WriteLine("2 - Скачать данные CDDIS");
            Console.WriteLine("3 - Скачать и обработать данные CDDIS");
            Console.WriteLine("4 - Обработать данные CDDIS");
            Console.WriteLine("5 - Скачать данные IERS");
            Console.WriteLine("6 - Скачать и обработать данные IERS");
            Console.WriteLine("7 - Обработать данные IERS");
            Console.WriteLine("8 - Тестировать все функции");
            Console.WriteLine("0 - Выйти");
        }
    }
}