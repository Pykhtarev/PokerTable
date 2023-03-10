using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3 };
            var table = new TableEquilibrium(arr);

            if (!table.IsEquilibriumAble)
            {
                Console.WriteLine("Wrong number of chips, can't equilibrium the table");
                Console.ReadKey();
            }
            else
            {
                table.MinEquilibriumMovesCalc();
                Console.WriteLine($"Min flip {table.MinEquilibriumMoves}");
                Console.ReadKey();
            }

        }
    }
}
