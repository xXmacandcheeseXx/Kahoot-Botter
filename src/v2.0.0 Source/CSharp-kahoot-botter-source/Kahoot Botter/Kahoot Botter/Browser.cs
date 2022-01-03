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

        public static bool GameOver = false;

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
            driver.Quit();
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

        public void Answer()
        {
            const string first_button = "//button[@data-functional-selector='answer-0']";
            const string second_button = "//button[@data-functional-selector='answer-1']";
            const string third_button = "//button[@data-functional-selector='answer-2']";
            const string fourth_button = "//button[@data-functional-selector='answer-3']";

            driver.SwitchTo().Window(driver.WindowHandles[0]);
            try
            {
                driver.FindElement(By.XPath("//p[@data-functional-selector='ranking-text']"));
                GameOver = true;
                goto END;
            }
            catch { }
        A:
            try
            {
                driver.FindElement(By.XPath(first_button));
            }
            catch { goto A; }

            try
            {
                string type = driver.FindElement(By.XPath("//span[@data-functional-selector='question-type-heading']")).Text;

                if (type.ToLower().Contains("true"))
                {
                    goto TrueOrFalse;
                }
                else if (type.ToLower().Contains("quiz"))
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
                    driver.FindElement(By.XPath(fourth_button));
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