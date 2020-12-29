using System;
using System.IO;
using System.Linq;

namespace _05
{
    class Program
    {
        static void Main(string[] args)
        {
            var seats = File.ReadAllLines("input.txt");
            Console.WriteLine(seats.Max(CalculateId));

            int min = seats.Min(CalculateId);
            var seatIds = seats.Select(CalculateId).ToHashSet();

            Console.WriteLine(Enumerable.Range(min, 1000).First(s => !seatIds.Contains(s)));
        }

        static int CalculateId(string seat)
        {
            var row = ToDecimal(seat.Substring(0, 7), 'F', 'B');
            var col = ToDecimal(seat.Substring(7), 'L', 'R');

            return row * 8 + col;
        }

        static int ToDecimal(string input, char zero, char one)
        {
            input = input.Replace(zero, '0');
            input = input.Replace(one, '1');

            return Convert.ToInt32(input, 2);
        }
    }
}
