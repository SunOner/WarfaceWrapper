using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarfaceWrapper
{
    public class Args_watcher
    {
        static public string bot_dir = "";
        public void init(string[] args)
        {
            Auth Auth = new Auth();
            string help_ = "Start example:\nWarfaceWrapper login password ru-alpha|ru-bravo|ru-charlie|ru-delta F:/warfacebot-master/";
            try
            {
                Auth.login = args[0];
            }
            catch
            {
                Console.WriteLine($"Login error\n{help_}");
                Console.ReadKey();
            }

            try
            {
                Auth.password = args[1];
            }
            catch
            {
                Console.WriteLine($"Password error\n{help_}");
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
                Console.WriteLine($"Server error\n{help_}");
                Console.ReadKey();
            }

            try
            {
                bot_dir = args[3];
                if (bot_dir.Contains("wb.exe")) bot_dir.Replace("wb.exe", "");
            }
            catch
            {
                Console.WriteLine($"Warface bot dir error\n{help_}");
                Console.ReadKey();
            }
            Console.Clear();

            Auth.Get_State_Cookies();
        }
    }
}
