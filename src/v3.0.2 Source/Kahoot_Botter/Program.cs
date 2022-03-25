using System;

namespace Kahoot_Botter
{
    class Program
    {
        static bool RunningWithArgs = false;

        static bool ShowBrowserOnTop = false;

        public static bool ShowBrowser;
        public static string GameCode = "";
        public static int NumberOfBots = -1;
        public static string Prefix = "";
        public static string PrefixValue = "";

        static void Main(string[] args)
        {
            if (args.Length == 4)
            {
                try
                {
                    if (CheckInfo(args[0], args[1], Convert.ToInt32(args[2]), args[3]) == true)
                    {
                        ShowBrowser = Convert.ToBoolean(args[0]);
                        GameCode = args[1];
                        NumberOfBots = Convert.ToInt32(args[2]);
                        Prefix = "Random";
                        RunningWithArgs = true;
                    }
                }
                catch
                { 
                    Console.WriteLine("Arguments are not in correct format");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            else if (args.Length == 5)
            {
                try
                {
                    if (CheckInfo(args[0], args[1], Convert.ToInt32(args[2]), args[3], args[4]) == true)
                    {
                        ShowBrowser = Convert.ToBoolean(args[0]);
                        GameCode = args[1];
                        NumberOfBots = Convert.ToInt32(args[2]);
                        Prefix = "Set";
                        PrefixValue = args[4];
                        RunningWithArgs = true;
                    }
                }
                catch
                {
                    Console.WriteLine("Arguments are not in correct format");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            else if (args.Length != 0 && args.Length != 4 && args.Length != 5)
            {
                Console.WriteLine("Invalid number of arguments");
                Console.ReadKey();
                Environment.Exit(0);
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Console.Title = "Kahoot Botter | v3.0.2";

            if (RunningWithArgs == true)
            {
                StartBotting();
            }

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

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        SHOWBROWSER:
            Console.Clear();
            Typecbp();
            Console.WriteLine("Show Browser during process? (Y/N) (you need to keep browser at a size where YOU are able to see and interact with the elements on the webpage but it fixes the bug where you can't answer if the game has \"Show Answers on players' devices\" turned on");

            string showbrowser = Console.ReadLine().ToLower();

            if (showbrowser == "y")
            {
                ShowBrowser = true;
            }
            else if (showbrowser == "n")
            {
                ShowBrowser = false;
            }
            else
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Try again");
                Console.ReadKey();
                goto SHOWBROWSER;
            }
            ShowBrowserOnTop = true;

            ///////////////////////////////////////////////////////////////////////////////

            Console.Clear();
            Typecbp();
            Console.WriteLine("Type the game code (no spaces)");
            string gamecodetest = Console.ReadLine();
        D:
            try
            {
                Convert.ToInt32(gamecodetest);

                if (gamecodetest.Length < 4 || gamecodetest.Length > 8)
                {
                    Console.Clear();
                    Typecbp();
                    Console.WriteLine("Try again");
                    Console.ReadKey();
                    Console.Clear();
                    Typecbp();
                    Console.WriteLine("Type the game code (no spaces)");
                    gamecodetest = Console.ReadLine();
                    Console.Clear();
                    goto D;
                }
            }
            catch
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Try again");
                Console.ReadKey();
                Console.Clear();
                Typecbp();
                Console.WriteLine("Type the game code (no spaces)");
                gamecodetest = Console.ReadLine();
                Console.Clear();
                goto D;
            }
        B:
            Console.Clear();
            Typecbp();
            Console.WriteLine("Are you sure \"" + gamecodetest + "\" is the game code? | (Y/N)");

            string yn = Console.ReadLine();

            if (yn.ToLower() == "n")
            {
                Console.Clear();
                Typecbp();
                Console.WriteLine("Type the game code (no spaces)");
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

            ///////////////////////////////////////////////////////////////////////////////

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

            ///////////////////////////////////////////////////////////////////////////////

        F:
            Console.Clear();
            Typecbp();
            Console.WriteLine("Do you want random prefix or set prefix? | (R/S)");

            string rs = Console.ReadLine();

            if (rs.ToLower() == "r")
            {
                Prefix = "Random";
            }
            else if (rs.ToLower() == "s")
            {
                Prefix = "Set";

                Console.Clear();
                Typecbp();
                Console.WriteLine("Type prefix for bots");
                string prtest = Console.ReadLine();

                if (prtest == "")
                {
                    Console.Clear();
                    goto F;
                }

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
            GC.Collect();

            ///////////////////////////////////////////////////////////////////////////////
        E:
            Typecbp();
            Console.WriteLine("Settings have been set\n\nType \"Start\" to send bots");

            string StartBot = Console.ReadLine();

            if (StartBot.ToLower() == "start")
            {
                GC.Collect();
                StartBotting();
            }
            else if (StartBot.ToLower() != "start")
            {
                Console.Clear();
                goto E;
            }
        }

        static bool CheckInfo(string ShowBrowser, string GameCode, int NumberOfBots, string Prefix, string PrefixValue = "")
        {
            Convert.ToBoolean(ShowBrowser);

            if (GameCode.Length < 4 || GameCode.Length > 8)
            {
                return false;
            }
            
            if (NumberOfBots < 1 || NumberOfBots > 100)
            {
                return false;
            }

            if (Prefix != "r" && Prefix != "s")
            {
                return false;
            }

            if (Prefix == "s" && PrefixValue == "")
            {
                return false;
            }

            return true;
        }

        static void Typecbp()
        {
            if (NumberOfBots == -1 && ShowBrowserOnTop == false)
            {
                Console.WriteLine("Show Browser:\nGame Code: " + GameCode + "\nBots:" + "\nPrefix: " + Prefix);
            }
            else if (NumberOfBots == -1 && ShowBrowserOnTop != false)
            {
                Console.WriteLine("Show Browser: " + ShowBrowser + "\nGame Code: " + GameCode + "\nBots:" + "\nPrefix: " + Prefix);
            }
            else if (NumberOfBots != -1)
            {
                Console.WriteLine("Show Browser: " + ShowBrowser + "Game Code: " + GameCode + "\nBots: " + NumberOfBots + "\nPrefix: " + Prefix);
            }

            if (Prefix == "Set")
            {
                Console.WriteLine("Prefix = \"" + PrefixValue + "\"");
            }

            Console.WriteLine("\n");
        }

        static void StartBotting()
        {
            Browser browser = new Browser();
            browser.StartBrowser();
            browser.Init();

            while (Browser.Stop == false)
            {
                if (Browser.times >= Browser.BotsToSend + 1)
                {
                    Browser.Stop = true;
                    break;
                }

                // --------------------------------------
                browser.InitTabs();
                // --------------------------------------

                Browser.times += 1;
                GC.Collect();
            }
            browser.SendBots();

            Console.Clear();
            Console.WriteLine("All bots successfully sent!\n\n");

            while (Browser.GameOver != true)
            {
                browser.Answer();
                Console.Clear();
                GC.Collect();
            }

            Console.Clear();
            GC.Collect();

            browser.CloseBrowser();

            Console.Clear();
        B:
            Console.WriteLine("Game has ended");

            while (true)
            {
                Console.ReadKey();
                Console.Clear();
                goto B;
            }
        }
    }
}