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

        static int gototab = 0;
        bool SpinForName = false;
        int NewTab = 0;
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

                    string Name = "";

                    if (Prefix == "Random")
                    {
                        Name = RandomString(50);

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
            string[] question = { };

            const string first_button = "//button[@data-functional-selector='answer-0']";
            const string second_button = "//button[@data-functional-selector='answer-1']";
            const string third_button = "//button[@data-functional-selector='answer-2']";
            const string fourth_button = "//button[@data-functional-selector='answer-3']"; // ANSWER BUTTONS
            const string submit_button = "//button[@data-functional-selector='jumble-submit-button']";
            const string submit_button2 = "//button[@data-functional-selector='text-answer-submit']";
            const string multi_sel_submit = "//button[@data-functional-selector='multi-select-submit-button']";
            const string text_input = "//textarea[@data-functional-selector='text-answer-input']";
            const string text_input2 = "//input[@data-functional-selector='text-answer-input']";
            const string vote_button = "//button[@data-functional-selector='voting-like-idea']";

        START:
            driver.SwitchTo().Window(driver.WindowHandles[gototab]);

            if (driver.Url.ToLower() == "https://kahoot.it/ranking") // DETECT IF GAME ENDS BY LINK CHANGING
            {
                GameOver = true;
                goto END;
            }

            if (driver.Url.ToLower() == "https://kahoot.it/")
            {
                gototab += 1;

                if (gototab + 1 >= BotsToSend)
                {
                    driver.Quit();
                    Console.Clear();
                    Console.WriteLine("All bots have been kicked from the game");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                goto START;
            }

            question = GetQuestion();

            if (question[0] == "tile" || question[0] == "mtile")
            {
                Random ran = new Random();

                for (int i = gototab; i < BotsToSend; i++)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[i]);

                    try
                    {
                        switch (new Random().Next(Convert.ToInt32(question[1])))
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

                        if (question[0] == "mtile")
                        {
                            driver.FindElement(By.XPath(multi_sel_submit)).Click();
                        }
                    }
                    catch { }
                }
            }
            else if (question[0] == "submit")
            {
                for (int i = gototab; i < BotsToSend; i++)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[i]);

                    try
                    {
                        driver.FindElement(By.XPath(submit_button)).Click();
                    }
                    catch { }
                }
            }
            else if (question[0] == "text")
            {
                for (int i = gototab; i < BotsToSend; i++)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[i]);

                    try
                    {
                        driver.FindElement(By.XPath(text_input)).SendKeys(RandomString(75));
                        driver.FindElement(By.XPath(submit_button2)).Click();
                    }
                    catch { }
                }
            }
            else if (question[0] == "text2")
            {
                for (int i = gototab; i < BotsToSend; i++)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[i]);

                    try
                    {
                        driver.FindElement(By.XPath(text_input2)).SendKeys(RandomString(75));
                        driver.FindElement(By.XPath(submit_button2)).Click();
                    }
                    catch { }
                }
            }
            else if (question[0] == "vote")
            {
                for (int i = gototab; i < BotsToSend; i++)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[i]);

                    try
                    {
                        driver.FindElement(By.XPath(vote_button)).Click();
                    }
                    catch { }
                }
            }

        END:
            Thread.Sleep(2000);
        }

        static string[] GetQuestion()
        {
            int numOfButtons = 0;
            const string first_button = "//button[@data-functional-selector='answer-0']";
            const string second_button = "//button[@data-functional-selector='answer-1']";
            const string third_button = "//button[@data-functional-selector='answer-2']";
            const string fourth_button = "//button[@data-functional-selector='answer-3']";
            const string submit_button = "//button[@data-functional-selector='jumble-submit-button']";
            const string multi_sel_submit = "//button[@data-functional-selector='multi-select-submit-button']";
            const string text_input = "//textarea[@data-functional-selector='text-answer-input']";
            const string text_input2 = "//input[@data-functional-selector='text-answer-input']";
            const string vote_button = "//button[@data-functional-selector='voting-like-idea']";

            try
            {
                driver.FindElement(By.XPath(first_button));
                numOfButtons += 1;
                driver.FindElement(By.XPath(second_button));
                numOfButtons += 1;
                driver.FindElement(By.XPath(third_button));
                numOfButtons += 1;
                driver.FindElement(By.XPath(fourth_button));

                try
                {
                    driver.FindElement(By.XPath(multi_sel_submit));
                    return new string[] { "mtile", "4" };
                }
                catch { }

                return new string[] { "tile", "4"};
            }
            catch
            {
                if (numOfButtons > 0)
                {
                    try
                    {
                        driver.FindElement(By.XPath(multi_sel_submit));
                        return new string[] { "mtile", numOfButtons.ToString() };
                    }
                    catch { }

                    return new string[] { "tile", numOfButtons.ToString() };
                }
            }

            try
            {
                driver.FindElement(By.XPath(submit_button));
                return new string[] { "submit" };
            }
            catch { }

            try
            {
                driver.FindElement(By.XPath(text_input));
                return new string[] { "text" };
            }
            catch { }

            try
            {
                driver.FindElement(By.XPath(text_input2));
                return new string[] { "text2" };
            }
            catch { }

            try
            {
                driver.FindElement(By.XPath(vote_button));
                return new string[] { "vote" };
            }
            catch { }

            return new string[] { "\0", "\0"};
        }

        static string RandomString(int length)
        {
            string RandomStr = "";

            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random ran = new Random();

            for (int e = 0; e < 50; e++)
            {
                RandomStr += characters.ElementAt(ran.Next(1, characters.Length));
            }

            return RandomStr;
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