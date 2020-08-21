using System;
using WarfaceWrapper;

namespace WarfaceAuth
{
    public class Debug_mod
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
        public void Write_NonDebug(string text = "")
        {
            Console.WriteLine($"{text}");
        }
    }
}