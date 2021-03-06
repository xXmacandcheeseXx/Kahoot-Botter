using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Kahoot_Botter
{
    public class Browser
    {
        string GameCode;
        public static int BotsToSend;
        string Prefix;
        string PrefixValue;

        int NewTab = 0;
        public static int times = 1;
        public static bool Stop = false;
        public static IWebDriver driver;
        IWebElement element;
        ChromeOptions options = new ChromeOptions();

        public void StartBrowser()
        {
            string dir = Directory.GetCurrentDirectory().Replace("\\", "/");
            string DriverDir = dir + "/drivers/Chrome";

            if (!Directory.Exists(DriverDir))
            {
                MissingDriver();
            }

            bool HasDriver = false;
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (string file in Directory.GetFiles(DriverDir))
                {
                    if (file.Contains("chromedriver"))
                    {
                        HasDriver = true;
                    }
                }

                if (HasDriver == false)
                {
                    MissingDriver();
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (!File.Exists(DriverDir + "/chromedriver.exe"))
                {
                    MissingDriver();
                }
            }

            options.AddArguments("--headless", "--disable-gpu", "--window-size=1440,900", "--silent");

            driver = new ChromeDriver(DriverDir, options);
        }

        public void SendBots()
        {
            for (int i = 0; i < times - 1; i++)
            {
            A:
                driver.SwitchTo().Window(driver.WindowHandles[i]);
                try
                {
                    element = driver.FindElement(By.XPath("//input[@name='nickname']"));
                    element.Click();
                }
                catch (Exception)
                {
                    goto A;
                }

                string Name = "";

                if (Prefix == "Random")
                {
                    string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                    Random random = new Random();

                    for (int e = 0; e < 15; e++)
                    {
                        Name += characters.ElementAt(random.Next(1, characters.Length));
                    }
                }
                else if (Prefix == "Set")
                {
                    Name = PrefixValue + "-" + (i + 1);
                }
                element.SendKeys(Name);
                element.SendKeys(Keys.Enter);
            }
        }

        public void CloseBrowser()
        {
            driver.Close();
            Environment.Exit(0);
        }

        public void InitTabs()
        {
            if (NewTab < 1)
            {
                driver.Navigate().GoToUrl("https://kahoot.it");
            }
            else if (NewTab > 0)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                driver.SwitchTo().Window(driver.WindowHandles.Last()).Navigate().GoToUrl("https://kahoot.it");
            }
        F:
            try
            {
                element = driver.FindElement(By.Name("gameId"));
            }
            catch (Exception)
            {
                goto F;
            }

            if (times < 2)
            {
                try
                {
                    element = driver.FindElement(By.XPath("//div[@data-functional-selector='notification - bar - text']"));
                    Console.Clear();
                    Console.WriteLine("Game code doesn't exist");
                    CloseBrowser();
                }
                catch (Exception)
                {
                    goto B;
                }
            }
        B:
            element.Click();
            element.SendKeys(GameCode.ToString());
            element.SendKeys(Keys.Enter);

            NewTab += 1;
        }

        public void Init()
        {
            if (Program.Prefix == "Set")
            {
                GameCode = Program.GameCode;
                BotsToSend = Program.NumberOfBots;
                Prefix = Program.Prefix;
                PrefixValue = Program.PrefixValue;
            }
            else if (Program.Prefix == "Random")
            {
                GameCode = Program.GameCode;
                BotsToSend = Program.NumberOfBots;
                Prefix = Program.Prefix;
            }
        }

        static void MissingDriver()
        {
            Console.Clear();
            Console.WriteLine("\"drivers\\Chrome\\chromedriver\" is missing | try downloading again from my github \"https://github.com/xXmacandcheeseXx/Kahoot-Botter\"");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}