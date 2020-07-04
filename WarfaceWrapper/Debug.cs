using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarfaceWrapper;

namespace WarfaceAuth
{
    public class Debug
    {
        public void Write_debug(string header, string text = "")
        {
            if (Program.Debug_mode == true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(header);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}