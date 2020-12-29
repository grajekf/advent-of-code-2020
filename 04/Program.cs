using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _04
{
    class Program
    {

        const string BirthYear = "byr";
        const string IssueYear = "iyr";
        const string ExpiraitionYear = "eyr";
        const string Height = "hgt";
        const string HairColor = "hcl";
        const string EyeColor = "ecl";
        const string PassportId = "pid";
        const string CountryId = "cid";

        static readonly string[] AllowedEyeColors = new string[]
        {
            "amb",
            "blu",
            "brn",
            "gry",
            "grn",
            "hzl",
            "oth",
        };


        static void Main(string[] args)
        {
            var passports = File.ReadAllText("input.txt")
                .Split("\r\n\r\n")
                .Select(p => p.Replace("\r\n", " "))
                .ToList();

            Console.WriteLine(passports.Count(p => HasRequiredFields(p)));
            Console.WriteLine(passports.Count(p => IsValid(p)));



        }

        static bool HasRequiredFields(string passport)
        {
            var partsDict = SplitIntoParts(passport);
            return HasRequiredFields(partsDict);
        }

        static IDictionary<string, string> SplitIntoParts(string passport)
        {
            return passport.Split(" ").Select(part =>
            {
                var partSplit = part.Split(":");
                return (partSplit[0], partSplit[1]);
            }).ToDictionary(p => p.Item1, p => p.Item2);
        }

        static bool HasRequiredFields(IDictionary<string, string> partsDict)
        {
            return
                partsDict.ContainsKey("byr")
                && partsDict.ContainsKey("iyr")
                && partsDict.ContainsKey("eyr")
                && partsDict.ContainsKey("hgt")
                && partsDict.ContainsKey("hcl")
                && partsDict.ContainsKey("ecl")
                && partsDict.ContainsKey("pid");
        }

        static bool IsValid(string passport)
        {
            var partsDict = SplitIntoParts(passport);

            if (!HasRequiredFields(partsDict))
                return false;

            if (!ValidateRange(partsDict[BirthYear], 1920, 2002))
                return false;
            if (!ValidateRange(partsDict[IssueYear], 2010, 2020))
                return false;
            if (!ValidateRange(partsDict[ExpiraitionYear], 2020, 2030))
                return false;

            if (!ValidateRangeWithSuffix(partsDict[Height], "cm", 150, 193)
                && !ValidateRangeWithSuffix(partsDict[Height], "in", 59, 76))
                return false;

            if (!ValidateHex(partsDict[HairColor]))
                return false;

            if (!AllowedEyeColors.Contains(partsDict[EyeColor]))
                return false;

            if (!ValidatePaddedNumber(partsDict[PassportId], 9))
                return false;

            return true;
        }

        static bool ValidateRange(string value, int min, int max)
        {
            if (!int.TryParse(value, out var valueParsed))
                return false;

            return valueParsed >= min && valueParsed <= max;
        }

        static bool ValidateRangeWithSuffix(string value, string suffix, int min, int max)
        {
            if (!value.EndsWith(suffix))
                return false;
            return ValidateRange(value.Replace(suffix, string.Empty), min, max);
        }

        static bool ValidateHex(string value)
        {
            return value[0] == '#' && value.Skip(1).All(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f'));
        }

        static bool ValidatePaddedNumber(string value, int length)
        {
            if (value.Length != length)
                return false;

            return value.All(char.IsNumber);
        }
    }

}
