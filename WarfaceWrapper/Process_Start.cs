using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace WarfaceWrapper
{
    public class Process_Start
    {
        Random HWID_Random = new Random();
        ProcessStartInfo PI;
        public void Start_Game(string uid,string token,string shard_id,string server,string dir)
        {
            try
            {
                Process exe = new Process();
                exe.StartInfo.FileName = dir + "Game.exe";
                exe.StartInfo.Arguments = $" -region_id global --shard_id={shard_id} -onlineserver {server} -uid {uid} -token {token}";
                exe.Start();
            }
            catch
            {
                Console.WriteLine("File Game.exe not found!");
                Console.ReadLine();
            }
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
                Arguments = $"/c cd /d {Program.exe_dir}" +
                $" & chcp 65001" +
                $" & wb.exe" +
                $" --id {uid}" +
                $" --token {token}" +
                $" -f {bot_server}" +
                $" -d game_hwid={HWID_Random.Next(100000000, 999999999)}",
                StandardOutputEncoding = Encoding.GetEncoding(866),
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var proc = Process.Start(PI);
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine() + " endl";

                #region Contains

                Match wb_nickname = Regex.Match(line, "Nickname: ([\\s\\S]+?) endl");
                if(wb_nickname.Success == true)
                {
                    string nickname = wb_nickname.Groups[1].Value;
                }

                #endregion

                if (line.Contains("Closed"))
                {
                    goto EndWhile;
                }
                Console.WriteLine(line);
            }
            EndWhile:
            proc.CancelOutputRead();
            proc.Kill();
            Auth Auth = new Auth();
            Auth.Get_State_Cookies();
        }
    }
}
