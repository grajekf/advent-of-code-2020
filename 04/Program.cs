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

            Console.WriteLine(passports.Count(p => IsValid(p)));

        }

        static bool IsValid(string passport)
        {
            var partsDict = passport.Split(" ").Select(part =>
            {
                var partSplit = part.Split(":");
                return (partSplit[0], partSplit[1]);
            }).ToDictionary(p => p.Item1, p => p.Item2);

            return
                partsDict.ContainsKey("byr")
                && partsDict.ContainsKey("iyr")
                && partsDict.ContainsKey("eyr")
                && partsDict.ContainsKey("hgt")
                && partsDict.ContainsKey("hcl")
                && partsDict.ContainsKey("ecl")
                && partsDict.ContainsKey("pid");
        }
    }

}
