using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarfaceAuth
{
    public class Process_Start
    {
        public void Start_Game(string uid,string token,string shard_id,string server,string dir)
        {
            Process exe = new Process();
            exe.StartInfo.FileName = dir;
            exe.StartInfo.Arguments = $" -region_id global --shard_id={shard_id} -onlineserver {server} -uid {uid} -token {token}";
            exe.Start();
        }
        public void Start_Bot(string uid,string token,string server,string dir)
        {

        }
    }
}
