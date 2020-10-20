using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_2070_5493
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome2070();
            Welcome5493();
            Console.ReadKey();
        }

        static partial void Welcome5493();

        private static void Welcome2070() 
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
