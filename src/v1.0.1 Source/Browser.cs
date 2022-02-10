using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;

namespace Kahoot_Botter
{
    public class Browser
    {
        string GameCode;
        public static int BotsToSend;
        string Prefix;
        string PrefixValue;

        int NewTab = 0;
        string DriverDir = "";
        string drive = "";
        public static int times = 1;
        public static bool Stop = false;
        public static IWebDriver driver;
        IWebElement element;
        ChromeOptions options = new ChromeOptions();

        public void StartBrowser()
        {
            var temp_folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp";

            DriverDir = File.ReadAllText(temp_folder + "\\KahootBotterDriverDir.txt");

            options.AddArguments("--headless", "--disable-gpu", "--window-size=1440,900");

            driver = new ChromeDriver(DriverDir, options);
        }

        public void SendBots()
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                goto F;
            }

            element.Click();
            element.SendKeys(GameCode.ToString());
            element.SendKeys(Keys.Enter);
        A:
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
                Name = PrefixValue + "-" + times;
            }
            element.SendKeys(Name);
            element.SendKeys(Keys.Enter);

            NewTab += 1;
        }

        public void CloseBrowser()
        {
            var temp_folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp";

            try
            {
                File.Delete(temp_folder + "\\KahootBotterDriverDir.txt");
            }
            catch (Exception) { }

            try
            {
                File.Delete(temp_folder + "\\KahootBotterGameInfo.txt");
            }
            catch (Exception) { }

            try
            {
                File.Delete(temp_folder + "\\KahootBotterBots.txt");
            }
            catch (Exception) { }

            try
            {
                File.Delete(drive + "ProgramData\\kahoot-button.html");
            }
            catch (Exception) { }

            driver.Close();
            Environment.Exit(0);
        }

        public void Init()
        {
            var temp_folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp";

            string[] GameInfo = File.ReadAllLines(temp_folder + "\\KahootBotterGameInfo.txt");

            if (GameInfo[0] == "true")
            {
                GameCode = GameInfo[1];
                BotsToSend = Convert.ToInt32(GameInfo[2]);
                Prefix = "Set";
                PrefixValue = GameInfo[3].ToString();
            }
            else if (GameInfo[0] != "true")
            {
                GameCode = GameInfo[0];
                BotsToSend = Convert.ToInt32(GameInfo[1]);
                Prefix = "Random";
            }
        }
    }
}