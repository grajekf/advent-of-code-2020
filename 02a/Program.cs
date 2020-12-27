using System;
using System.IO;
using System.Linq;

namespace _02a
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var passwords = lines.Select(l => PasswordWithRule.Parse(l)).ToList();
            Console.WriteLine(passwords.Count(p => p.IsValid()));
            Console.WriteLine(passwords.Count(p => p.IsValidSecond()));
        }
    }

    class PasswordWithRule
    {
        public PasswordWithRule(int from, int to, char character, string password)
        {
            From = from;
            To = to;
            Character = character;
            Password = password;
        }

        public int From { get; set; }
        public int To { get; set; }
        public char Character { get; set; }
        public string Password { get; set; }

        public static PasswordWithRule Parse(string input)
        {
            var parts = input.Split(" ");
            var fromToPart = parts[0];
            var charPart = parts[1];
            var passwordPart = parts[2];

            var fromToSplit = fromToPart.Split("-");
            var from = int.Parse(fromToSplit[0]);
            var to = int.Parse(fromToSplit[1]);

            var character = charPart[0];

            return new PasswordWithRule(from, to, character, passwordPart);
        }

        public bool IsValid()
        {
            var charCount = Password.Count(c => c == Character);
            return charCount >= From && charCount <= To;
        }

        public bool IsValidSecond()
        {
            var equalCount = 0;
            if (Password[From - 1] == Character)
                equalCount++;
            if (Password[To - 1] == Character)
                equalCount++;

            return equalCount == 1;
        }
    }
}
