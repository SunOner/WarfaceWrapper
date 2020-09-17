using EnvDTE;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WarfaceAuth;

namespace WarfaceWrapper
{
    public class Process_Start
    {
        StreamWriter Writer;
        Random HWID_Random = new Random();
        ProcessStartInfo PI;
        public void Init_Tools()
        {
            Debug_mod debug_mod = new Debug_mod();
            debug_mod.Write_debug("Init tools");
        }

        public void BotStart(string uid, string token, string server, string dir)
        {
            
            string bot_server = "./cfg/server/ru-alpha.cfg";
            if (server == "s1.warface.ru") { bot_server = "./cfg/server/ru-alpha.cfg"; }
            if (server == "s2.warface.ru") { bot_server = "./cfg/server/ru-bravo.cfg"; }
            if (server == "s3.warface.ru") { bot_server = "./cfg/server/ru-charlie.cfg"; }
            if (server == "s12.warface.ru") { bot_server = "./cfg/server/ru-delta.cfg"; }


            PI = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c cd /d {Args_watcher.bot_dir}" +
                $" & chcp 65001" +
                $" & wb.exe" +
                $" --id {uid}" +
                $" --token {token}" +
                $" -f {bot_server}" +
                $" -d game_hwid={HWID_Random.Next(100000000, 999999999)}",
                StandardOutputEncoding = Encoding.GetEncoding(866),
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };
            
            var proc = System.Diagnostics.Process.Start(PI);
            Writer = proc.StandardInput;
            while (!proc.StandardOutput.EndOfStream)
            {
                string[] line = new Regex(@"\x1B\[[^@-~]*[@-~]|CMD#\s*").Replace(proc.StandardOutput.ReadLine(), "").Split(' ');
                switch(line[0])
                {
                    case "Closed":
                        if(line[1] == "readstream" || line[1] == "sendstream" || line[1] == "ping")
                        {
                            Writer.WriteLine(" ");
                        }
                        break;
                    case "Nickname:":
                        Console.Title = Encoding.UTF8.GetString(Encoding.GetEncoding(866).GetBytes(line[1]));
                        break;
                    case "Joined":
                        if(string.Join(" ", line).Contains("channel"))
                        {
                            Console.Title = Console.Title + " | " + line[2];
                        }
                        break;
                    default:
                        Console.WriteLine(string.Join(" ", line));
                        break;
                }
            }
            proc.Kill();
            //proc.CancelOutputRead();
            Auth Auth = new Auth();
            Auth.Get_State_Cookies();
        }
    }
}
