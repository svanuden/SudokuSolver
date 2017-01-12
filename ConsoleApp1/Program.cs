using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new Grid();
            grid.SetValue(0, 0, 3);
            grid.SetValue(5, 0, 8);
            grid.SetValue(0, 1, 7);
            grid.SetValue(2, 1, 8);
            grid.SetValue(3, 1, 3);
            grid.SetValue(4, 1, 2);
            grid.SetValue(8, 1, 5);
            grid.SetValue(3, 2, 9);
            grid.SetValue(7, 2, 1);
            grid.SetValue(0, 3, 9);
            grid.SetValue(5, 3, 4);
            grid.SetValue(7, 3, 2);
            grid.SetValue(4, 4, 1);
            grid.SetValue(1, 5, 7);
            grid.SetValue(3, 5, 8);
            grid.SetValue(8, 5, 9);
            grid.SetValue(1, 6, 5);
            grid.SetValue(5, 6, 3);
            grid.SetValue(0, 7, 8);
            grid.SetValue(4, 7, 4);
            grid.SetValue(5, 7, 7);
            grid.SetValue(6, 7, 5);
            grid.SetValue(8, 7, 3);
            grid.SetValue(3, 8, 5);
            grid.SetValue(8, 8, 6);
            Console.Clear();
            Console.WriteLine(grid.Draw());
            Console.ReadKey();
            var rerun = true;
            while (rerun)
            {
                rerun = grid.Process();
                Console.Clear();
                Console.WriteLine(grid.Draw());
                Console.WriteLine("Rerun: " + rerun);
                Console.WriteLine("Give coords for possible values:");
                var input = Console.ReadLine();
                var x = 0;
                var y = 0;
                if (input != null && input.Length == 2 && 
                    int.TryParse(input[0].ToString(), out x) && 
                    int.TryParse(input[1].ToString(), out y))
                {
                    Console.WriteLine(string.Join(",",grid.GetPossibleValuesOfCoord(x,y)));
                    Console.ReadLine();
                    rerun = true;
                }
            }
        }
    }
}
