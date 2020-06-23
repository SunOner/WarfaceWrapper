using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WarfaceAuth
{
    public class Program
    {
        static public bool Start_game = true;
        static public string exe_dir = "";
        static void Main(string[] args)
        {
            Auth Auth = new Auth();

            
            Auth.login = args[0];
            Auth.password = args[1];
            if(args[2] == "ru-alpha")
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
            if(args[3] == "g")
            {
                Start_game = true;
            }
            if (args[3] == "b")
            {
                Start_game = false;
            }
            exe_dir = args[4];

            Console.WriteLine($"Log:{Auth.login} Pass:{Auth.password}");
            Auth.Get_State_Cookies();
            Console.ReadLine();
        }
    }
}
