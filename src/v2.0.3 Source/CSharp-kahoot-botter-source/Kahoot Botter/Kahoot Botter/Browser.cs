using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kahoot_Botter
{
    public class Browser
    {
        string GameCode;
        public static int BotsToSend;
        string Prefix;
        string PrefixValue;

        public static bool GameOver = false;

        bool SpinForName = false;
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

            options.AddArguments("--headless", "--disable-gpu", "--silent");

            driver = new ChromeDriver(DriverDir, options);
        }

        public void SendBots()
        {
            if (SpinForName == true) // IF GAME HAS FRIENDLY NICKNAMES ON
            {
                for (int i = 0; i < BotsToSend; i++)
                {
                A:
                    driver.SwitchTo().Window(driver.WindowHandles[i]);
                    try
                    {
                        driver.FindElement(By.XPath("//button[@data-functional-selector='namerator-spin-button']")).Click(); // SPIN FOR NAME
                    }
                    catch { goto A; }
                }

                for (int ii = 0; ii < BotsToSend; ii++)
                {
                A:
                    driver.SwitchTo().Window(driver.WindowHandles[ii]);
                    try
                    {
                        driver.FindElement(By.XPath("//button[@data-functional-selector='namerator-continue-button']")).Click(); // CLICK OK
                    }
                    catch { goto A; }
                }
                goto END;
            }
            else if (SpinForName == false) // IF GAME DOESN'T HAVE FRIENDLY NICKNAMES ON
            {
                for (int i = 0; i < BotsToSend; i++)
                {
                A:
                    driver.SwitchTo().Window(driver.WindowHandles[i]);
                    try
                    {
                        element = driver.FindElement(By.XPath("//input[@name='nickname']"));
                        element.Click();
                    }
                    catch { goto A; }

                    string Name = ""; // GENERATE RANDOM NAME

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
                }
                for (int ii = 0; ii < BotsToSend; ii++)
                {
                A:
                    driver.SwitchTo().Window(driver.WindowHandles[ii]);
                    try
                    {
                        driver.FindElement(By.XPath("//input[@name='nickname']")).SendKeys(Keys.Enter);
                    }
                    catch { goto A; }
                }
            }
        END:;
        }

        public void CloseBrowser()
        {
            driver.Quit();
        }

        public void InitTabs()
        {
            if (NewTab < 1)
            {
                driver.Navigate().GoToUrl("https://kahoot.it?pin=" + GameCode);
            }
            else if (NewTab > 0)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                driver.SwitchTo().Window(driver.WindowHandles.Last()).Navigate().GoToUrl("https://kahoot.it?pin=" + GameCode);
            }

            if (NewTab < 1)
            {
                bool CodeExists = false;
                Thread.Sleep(1750);

                try
                {
                    driver.FindElement(By.XPath("//button[@data-functional-selector='namerator-spin-button']")); // SPIN BUTTON IF WHEEL SPIN FOR NAMES IS ON
                    SpinForName = true;

                    if (CodeExists != true)
                    {
                        CodeExists = true;
                    }
                }
                catch { }
                try
                {
                    driver.FindElement(By.XPath("//input[@name='nickname']")); // NICKNAME BOX

                    if (CodeExists != true)
                    {
                        CodeExists = true;
                    }
                }
                catch { }

                if (CodeExists == false)
                {
                    CloseBrowser();
                    Console.Clear();

                    while (true)
                    {
                        Console.WriteLine("Game code doesn't exist");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            NewTab += 1;
        }

        public void Answer()
        {
            const string first_button = "//button[@data-functional-selector='answer-0']";
            const string second_button = "//button[@data-functional-selector='answer-1']";
            const string third_button = "//button[@data-functional-selector='answer-2']";
            const string fourth_button = "//button[@data-functional-selector='answer-3']"; // ANSWER BUTTONS

            driver.SwitchTo().Window(driver.WindowHandles[0]);

        A:
            if (driver.Url.ToLower() == "https://kahoot.it/ranking") // DETECT IF GAME ENDS BY LINK CHANGING
            {
                GameOver = true;
                goto END;
            }

            try
            {
                driver.FindElement(By.XPath(first_button));
            }
            catch { goto A; }

            try
            {
                string type = driver.FindElement(By.XPath("//span[@data-functional-selector='question-type-heading']")).Text; // THE TOP BAR ABOVE THE QUESTION

                if (type.ToLower().Contains("true")) // IF TOP BAR CONTAINS "true" IT IS TRUE OR FALSE
                {
                    goto TrueOrFalse;
                }
                else if (type.ToLower() == "quiz") // IF TOP BAR EQUALS "quiz" IT IS QUIZ
                {
                    goto FourQuestion;
                }
                else
                {
                    goto END;
                }
                // QUESTION TYPES GO HERE
            }
            catch { goto A; }

        FourQuestion:
            for (int i = 0; i < BotsToSend; i++)
            {
                driver.SwitchTo().Window(driver.WindowHandles[i]);
            C:
                try
                {
                    driver.FindElement(By.XPath(fourth_button)); // DETECT IF IT'S A 3 QUESTION ANSWER
                    switch (new Random().Next(1, 5))
                    {
                        case 1:
                            driver.FindElement(By.XPath(first_button)).Click();
                            break;
                        case 2:
                            driver.FindElement(By.XPath(second_button)).Click();
                            break;
                        case 3:
                            driver.FindElement(By.XPath(third_button)).Click();
                            break;
                        case 4:
                            driver.FindElement(By.XPath(fourth_button)).Click();
                            break;
                        default:
                            goto C;

                    }
                }
                catch
                {
                    try
                    {
                        switch (new Random().Next(1, 4))
                        {
                            case 1:
                                driver.FindElement(By.XPath(first_button)).Click();
                                break;
                            case 2:
                                driver.FindElement(By.XPath(second_button)).Click();
                                break;
                            case 3:
                                driver.FindElement(By.XPath(third_button)).Click();
                                break;
                            default:
                                goto C;
                        }
                    }
                    catch
                    {
                        goto END;
                    }
                }
            }
            goto END;
        TrueOrFalse:
            for (int i = 0; i < BotsToSend; i++)
            {
                driver.SwitchTo().Window(driver.WindowHandles[i]);
                try
                {
                B:
                    switch (new Random().Next(1, 3))
                    {
                        case 1:
                            driver.FindElement(By.XPath(first_button)).Click();
                            break;
                        case 2:
                            driver.FindElement(By.XPath(second_button)).Click();
                            break;
                        default:
                            goto B;
                    }
                }
                catch { goto END; }
            }
            goto END;

        END:;
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