using System;
using System.IO;
using System.Text;

namespace Kahoot_Botter
{
    class Program
    {
        public static string GameCode = "";
        public static int NumberOfBots = -1;
        public static string Prefix = "";
        public static string PrefixValue = "";
        public static bool SetPrefix = false;

        static void Main()
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
            Console.WriteLine("Type the game code (no spaces)");
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
                    Console.WriteLine("Type the game code (no spaces)");
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
                Console.WriteLine("Type the game code (no spaces)");
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
            Console.WriteLine("Do you want random prefix or set prefix? | (R/S)");

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
            GC.Collect();
            StartBotting();
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
        A:
            Console.WriteLine("Settings have been set\n\nNOTE: this takes up about 1.75 GB of RAM per 100 bots\n\nType \"Start\" to send bots");

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
                if (Browser.times >= Browser.BotsToSend + 1)
                {
                    Browser.Stop = true;
                    break;
                }

                // --------------------------------------
                browser.SendBots();
                // --------------------------------------

                Browser.times += 1;
                GC.Collect();
            }
        B:
            Console.Clear();
            Console.WriteLine("All bots successfully sent!\n\nClose the application to remove all bots\n\n");

            while (true)
            {
                Console.ReadKey();
                goto B;
            }
        }
    }
}