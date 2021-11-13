using System;
using System.IO;
using System.Threading;

namespace Kahoot_Botter
{
    class Program
    {
        public static string GameCode = "";
        public static int NumberOfBots = -1;
        public static string Prefix = "";
        public static string PrefixValue = "";
        public static bool SetPrefix = false;

        static void Main(string[] args)
        {
            Console.Title = "Kahoot Botter";
        A:
            Console.WriteLine("NOTE: GOOGLE CHROME NEEDS TO BE INSTALLED FOR THIS TO WORK\n\nType \"Start\" to begin bot process");

            string didtypestart = Console.ReadLine();

            if (didtypestart.ToLower() != "start")
            {
                Console.Clear();
                Console.WriteLine("Try again");
                Console.ReadKey();
                Console.Clear();
                goto A;
            }

            Console.Clear();
            Typecbp();
            Console.WriteLine("Type the game code");
            string gamecodetest = Console.ReadLine();
        D:
            try
            {
                Convert.ToInt32(gamecodetest);

                if (gamecodetest.Length < 5 || gamecodetest.Length > 8)
                {
                    Console.Clear();
                    Typecbp();
                    Console.WriteLine("Try again");
                    Console.ReadKey();
                    Console.Clear();
                    Typecbp();
                    Console.WriteLine("Type the game code");
                    gamecodetest = Console.ReadLine();
                    Console.Clear();
                    goto D;
                }
            }
            catch (FormatException)
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Try again");
                Console.ReadKey();
                Console.Clear();
                Typecbp();
                Console.WriteLine("Type the game code");
                gamecodetest = Console.ReadLine();
                Console.Clear();
                goto D;
            }
            catch (OverflowException)
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Try again");
                Console.ReadKey();
                Console.Clear();
                Typecbp();
                Console.WriteLine("Type the game code");
                gamecodetest = Console.ReadLine();
                Console.Clear();
                goto D;
            }
        B:
            Console.Clear();
            Typecbp();
            Console.WriteLine("Are you sure \"" + gamecodetest + "\" is the game code? | Y/N");

            string yn = Console.ReadLine();

            if (yn.ToLower() == "n")
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Type the game code");
                gamecodetest = Console.ReadLine();
                Console.Clear();
                goto B;
            }

            else if (yn.ToLower() != "y" && yn.ToLower() != "n")
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Try again");
                Console.ReadKey();
                Console.Clear();
                goto B;
            }
            GameCode = gamecodetest;
            int botnumbertest;
        C:
            Console.Clear();
            Typecbp();
            Console.WriteLine("How many bots do you want to send? (100 max)");
            try
            {
                botnumbertest = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Try again");
                Console.ReadKey();
                Console.Clear();
                goto C;
            }

            if (botnumbertest < 1)
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Number too small");
                Console.ReadKey();
                Console.Clear();
                goto C;
            }
            else if (botnumbertest > 100)
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Number too big");
                Console.ReadKey();
                Console.Clear();
                goto C;
            }
            NumberOfBots = botnumbertest;
        F:
            Console.Clear();
            Typecbp();
            Console.WriteLine("Do you want random prefix or set prefix? | R/S");

            string rs = Console.ReadLine();

            if (rs.ToLower() == "r")
            {
                Prefix = "Random";
            }
            else if (rs.ToLower() == "s")
            {
                Prefix = "Set";
            E:
                Console.Clear();
                Typecbp();
                Console.WriteLine("Type prefix for bots");
                string prtest = Console.ReadLine();
                
                if (prtest.Length < 1)
                {
                    Console.Clear();
                    Typecbp();
                    Console.WriteLine("Prefix needs to be at least 1 character long");
                    Console.ReadKey();
                    goto E;
                }
                else if (prtest.Length > 11)
                {
                    Console.Clear();
                    Typecbp();
                    Console.WriteLine("Prefix needs to be less than or equal to 11 characters");
                    Console.ReadKey();
                    goto E;
                }
                SetPrefix = true;
                PrefixValue = prtest;
            }
            else
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Try again");
                Console.ReadKey();
                Console.Clear();
                goto F;
            }

            Console.Clear();
            Typecbp();
        G:
            Console.WriteLine("Type \"Set\" to set settings");
            string StartBot = Console.ReadLine();

            if (StartBot.ToLower() == "set")
            {
                Console.Clear();
                GC.Collect();
                StartBotting();
            }
            else if (StartBot.ToLower() != "set")
            {
                Console.Clear();
                Typecbp();
                goto G;
            }
        }

        static void Typecbp()
        {
            if (NumberOfBots == -1)
            {
                Console.WriteLine("Game Code: " + GameCode + "\nBots:" + "\nPrefix: " + Prefix);
            }
            else if (NumberOfBots != -1)
            {
                Console.WriteLine("Game Code: " + GameCode + "\nBots: " + NumberOfBots + "\nPrefix: " + Prefix);
            }

            if (SetPrefix == true)
            {
                Console.WriteLine("Prefix = \"" + PrefixValue + "\"");
            }
            Console.WriteLine("\n");
        }

        static void StartBotting()
        {
            var temp_folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp";

            var DriverDir = temp_folder + "\\KahootBotterDriverDir.txt";
            var GameInfo = temp_folder + "\\KahootBotterGameInfo.txt";
            var SentBotsCount = temp_folder + "\\KahootBotterBots.txt";

            if (!File.Exists(DriverDir))
            {
                File.Create(DriverDir).Close();
            }
            File.WriteAllText(DriverDir, Directory.GetCurrentDirectory() + "\\drivers\\Chrome");

            if (!File.Exists(GameInfo))
            {
                File.Create(GameInfo).Close();
            }

            File.WriteAllText(GameInfo, "");
            using (TextWriter tw = new StreamWriter(GameInfo))
            {
                if (SetPrefix == true)
                {
                    tw.WriteLine("true");
                    tw.WriteLine(GameCode);
                    tw.WriteLine(NumberOfBots);
                    tw.WriteLine(PrefixValue);
                }
                else if (SetPrefix == false)
                {
                    tw.WriteLine(GameCode);
                    tw.WriteLine(NumberOfBots);
                }
            }

            if (!File.Exists(SentBotsCount))
            {
                File.Create(SentBotsCount).Close();
            }
            File.WriteAllText(SentBotsCount, "0");
        A:
            Console.WriteLine("Settings have been set\n\nNOTE: this takes up about 1.75 GB of RAM per 100 bots\n\nType \"Start\" to start botting");

            string StartBot = Console.ReadLine();

            if (StartBot.ToLower() == "start")
            {
                GC.Collect();
                goto C;
            }
            else if (StartBot.ToLower() != "start")
            {
                Console.Clear();
                goto A;
            }
        C:
            Browser browser = new Browser();
            browser.StartBrowser();
            browser.Init();

            while (Browser.Stop == false)
            {
                int TimeoutTimes = 0;
            B:
                try
                {
                    Browser.times = Convert.ToInt32(File.ReadAllText(temp_folder + "\\KahootBotterBots.txt"));

                    if (Browser.times >= Browser.BotsToSend)
                    {
                        Browser.Stop = true;
                        break;
                    }
                }
                catch (Exception)
                {
                    TimeoutTimes += 1;

                    if (TimeoutTimes == 10)
                    {
                        Browser.Stop = true;
                        break;
                    }
                    Thread.Sleep(100);
                    goto B;
                }

                // --------------------------------------
                browser.SendBots();
                // --------------------------------------

                int TimeoutTimes2 = 0;
            E:
                try
                {
                    Browser.times = Convert.ToInt32(File.ReadAllText(temp_folder + "\\KahootBotterBots.txt"));
                    File.WriteAllText(temp_folder + "\\KahootBotterBots.txt", (Browser.times + 1).ToString());
                }
                catch (Exception)
                {
                    TimeoutTimes2 += 1;

                    if (TimeoutTimes2 == 10)
                    {
                        Browser.Stop = true;
                        break;
                    }
                    Thread.Sleep(100);
                    goto E;
                }
                GC.Collect();
            }
            browser.CloseBrowser();
        }
    }
}