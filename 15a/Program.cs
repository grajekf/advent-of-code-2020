using System;
using System.Linq;
using System.Collections.Generic;

namespace _15a
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int> { 9, 3, 1, 0, 8, 4 };
            // List<int> numbers = new List<int> { 0, 3, 6 };

            for (int i = numbers.Count; i <= 2019; i++)
            {
                var lastNumber = numbers.Last();
                var index = numbers.Take(numbers.Count - 1).ToList().LastIndexOf(lastNumber);
                if (index == -1)
                {
                    numbers.Add(0);
                }
                else
                {
                    numbers.Add(i - index - 1);
                }
            }

            Console.WriteLine(numbers.Last());
        }
    }
}
