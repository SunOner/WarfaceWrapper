using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace WarfaceWrapper
{
    public class Program
    {
        public static bool Debug_mode = true;
        static void Main(string[] args)
        {
            Args_watcher args_Watcher = new Args_watcher();
            args_Watcher.init(args);
        }
    }
}
