using System;
using System.IO;
using System.Linq;

namespace _06
{
    class Program
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        static void Main(string[] args)
        {
            var groups = File.ReadAllText("input.txt").Split("\r\n\r\n").Select(g => g.Split("\r\n"));
            Console.WriteLine(groups
                .Sum(g => g.Aggregate(string.Empty.Select(c => c), (u, answers) => u.Union(answers), r => r.Count())));
            Console.WriteLine(groups
              .Sum(g => g.Aggregate(alphabet.Select(c => c), (u, answers) => u.Intersect(answers), r => r.Count())));
        }
    }
}
