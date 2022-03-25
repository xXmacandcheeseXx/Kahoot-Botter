using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;
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
            string DriverDir = Directory.GetCurrentDirectory() + "\\drivers\\Chrome";

            if (!File.Exists(DriverDir + "\\chromedriver.exe"))
            {
                MissingDriver();
            }

            if (Program.ShowBrowser == false)
            {
                options.AddArguments("--headless", "--disable-gpu", "--silent");
            }

            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalCapability("useAutomationExtension", false);

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

                        for (int e = 0; e < 50; e++)
                        {
                            Name += characters.ElementAt(random.Next(1, characters.Length));
                        }

                        element = driver.FindElement(By.XPath("//input[@maxlength='15']"));
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].setAttribute('maxlength','');", element); // remove character limit from nickname box
                    }
                    else if (Prefix == "Set")
                    {
                        Name = PrefixValue + "-" + (i + 1);

                        if (PrefixValue.Length > 11)
                        {
                            element = driver.FindElement(By.XPath("//input[@maxlength='15']"));
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].setAttribute('maxlength','');", element); // remove character limit for prefix box if name is greater than 11 characters (because if it is lower there is no need)
                        }
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

            for (int i = 0; i < BotsToSend; i++)
            {
            ERROR:
                driver.SwitchTo().Window(driver.WindowHandles[i]);

                try
                {
                    switch (new Random().Next(4))
                    {
                        case 0:
                            driver.FindElement(By.XPath(first_button)).Click();
                            break;
                        case 1:
                            driver.FindElement(By.XPath(second_button)).Click();
                            break;
                        case 2:
                            driver.FindElement(By.XPath(third_button)).Click();
                            break;
                        case 3:
                            driver.FindElement(By.XPath(fourth_button)).Click();
                            break;
                    }
                }
                catch { goto ERROR; }
            }

        END:
            Thread.Sleep(1);
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
            Console.WriteLine("\"drivers\\Chrome\\chromedriver.exe\" is missing | try downloading from chromium \"https://chromedriver.chromium.org/downloads\"");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}