using System;
using System.Linq;
using System.Numerics;

namespace _13b
{
    class Program
    {
        static void Main(string[] args)
        {
            int earliestDeparture = 1000340;
            string timeString = "13,x,x,x,x,x,x,37,x,x,x,x,x,401,x,x,x,x,x,x,x,x,x,x,x,x,x,17,x,x,x,x,19,x,x,x,23,x,x,x,x,x,29,x,613,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,41";
            var splittedIds = timeString.Split(',');
            var validIds = splittedIds
                .Zip(Enumerable.Range(0, splittedIds.Length))
                .Where(x => x.First != "x")
                .Select(x => (Id: int.Parse(x.First), Minutes: x.Second))
                .Zip("abcdefghijklmnopqrstuvwxyz")
                .Select(x => (Id: x.First.Id, Minutes: x.First.Minutes, Letter: x.Second))
                .ToList();

            BigInteger currentNumber = 13;
            BigInteger currentInterval = 13;

            foreach (var x in validIds.Skip(1))
            {
                var modulo = GetModulo(x);
                while (currentNumber % x.Id != modulo)
                    currentNumber += currentInterval;

                Console.WriteLine(currentNumber);
                currentInterval = currentInterval * x.Id;
            }
        }

        static int GetModulo((int Id, int Minutes, char Letter) x)
        {
            var modulo = x.Id - (x.Minutes % x.Id);

            return modulo % x.Id;
        }
    }
}
