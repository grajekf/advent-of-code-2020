using System;
using System.IO;
using System.Linq;

namespace _04
{
    class Program
    {
        static void Main(string[] args)
        {
            var passports = File.ReadAllText("input.txt")
                .Split("\r\n\r\n")
                .Select(p => p.Replace("\r\n", " "))
                .ToList();

        }
    }
}
