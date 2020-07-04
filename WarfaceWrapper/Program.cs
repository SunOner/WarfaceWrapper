using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WarfaceWrapper
{
    public class Program
    {
        public static bool Debug_mode = true;


        static public bool Start_game = true;
        static public string exe_dir = "";
        static void Main(string[] args)
        {
            Auth Auth = new Auth();
            string help_ = "example:\nWarfaceAuth <login> <password> <ru-alpha|ru-bravo|ru-charlie|ru-delta> <g> <F:/Warface/Bin64Release/> (for game) or \nWarfaceAuth <login> <password> <ru-alpha|ru-bravo|ru-charlie|ru-delta> <b> <F:/warfacebot-master/> (for warface bot)";
            try
            {
                Auth.login = args[0];
            }
            catch
            {
                Console.WriteLine($"Need a login..\n{help_}");
                Console.ReadKey();
            }
            
            try
            {
                Auth.password = args[1];
            }
            catch
            {
                Console.WriteLine($"Need a password..\n{help_}");
                Console.ReadKey();
            }
            
            try
            {
                if (args[2] == "ru-alpha")
                {
                    Auth.shardid = "0";
                    Auth.server = "s1.warface.ru";
                }
                if (args[2] == "ru-bravo")
                {
                    Auth.shardid = "1";
                    Auth.server = "s2.warface.ru";
                }
                if (args[2] == "ru-charlie")
                {
                    Auth.shardid = "2";
                    Auth.server = "s3.warface.ru";
                }
                if (args[2] == "ru-delta")
                {
                    Auth.shardid = "3";
                    Auth.server = "s12.warface.ru";
                }
            }
            catch
            {
                Console.WriteLine($"Need a server..\n{help_}");
                Console.ReadKey();
            }
            
            if (args[3] == "g")
            {
                Start_game = true;
            }
            if (args[3] == "b")
            {
                Start_game = false;
            }
            try
            {
                exe_dir = args[4];
            }
            catch
            {
                Console.WriteLine($"Need a dir path..\n{help_}");
                Console.ReadKey();
            }
            Auth.Get_State_Cookies();
        }
    }
}
