using System;
using System.IO;

namespace _03
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            bool[,] slope = new bool[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    slope[i, j] = line[j] == '#';
                }
            }

            Console.WriteLine(CountTrees(slope, 3, 1));


        }

        static int CountTrees(bool[,] slope, int dx, int dy)
        {
            var x = 0;
            var y = 0;

            var treesCount = 0;

            while (y < slope.GetLength(0))
            {
                if (slope[y, x])
                    treesCount++;

                x = (x + dx) % slope.GetLength(1);
                y += dy;
            }

            return treesCount;
        }
    }
}
