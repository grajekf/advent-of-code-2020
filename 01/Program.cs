using System;
using System.IO;
using System.Linq;

namespace _01
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = File.ReadAllLines("input.txt").Select(int.Parse).ToList();

            for (int i = 0; i < inputList.Count; i++)
            {
                var a = inputList[i];
                for (int j = i + 1; j < inputList.Count; j++)
                {
                    var b = inputList[j];
                    if (a + b == 2020)
                        Console.WriteLine(a * b);
                }
            }
        }
    }
}
