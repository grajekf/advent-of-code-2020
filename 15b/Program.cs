using System;
using System.Linq;
using System.Collections.Generic;

namespace _15b
{
    class Program
    {
        static void Main(string[] args)
        {
            List<long> numbers = new List<long> { 9, 3, 1, 0, 8, 4 };
            Dictionary<long, long> lastIndex = new Dictionary<long, long>();
            foreach (var x in numbers.Take(numbers.Count - 1).Zip(Enumerable.Range(1, numbers.Count - 1)))
            {
                lastIndex[x.First] = x.Second;
            }

            long lastNumber = numbers.Last();

            for (long i = numbers.Count; i < 30000000; i++)
            {
                long nextNumber;
                if (!lastIndex.ContainsKey(lastNumber))
                {
                    nextNumber = 0;
                } 
                else
                {
                    nextNumber = i - lastIndex[lastNumber];
                }
                lastIndex[lastNumber] = i;
                lastNumber = nextNumber;
            }

            Console.WriteLine(lastNumber);
        }
    }
}
