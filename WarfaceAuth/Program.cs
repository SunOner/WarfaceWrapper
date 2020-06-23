using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarfaceAuth
{
    class Program
    {
        static void Main(string[] args)
        {
            Auth Auth = new Auth();

            
            Auth.login = args[0];
            Auth.password = args[1];
            
            Console.WriteLine($"Log:{Auth.login} Pass:{Auth.password}");
            Auth.Get_State_Cookies();
            Console.ReadLine();
        }
    }
}
