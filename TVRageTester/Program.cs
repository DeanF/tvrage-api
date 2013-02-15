using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRageAPI;

namespace TVRageTester
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine(TVRage.Instance.FindShow("once upon a time"));
            Console.ReadLine();
        }
    }
}
