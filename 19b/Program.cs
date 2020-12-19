using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _19b
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("input.txt");
            var inputSplit = inputString.Split("\r\n\r\n");

            var ruleLines = inputSplit[0].Split("\r\n");
            var messageLines = inputSplit[1].Split("\r\n");

            var rules = ruleLines.SelectMany(r => Rule.Parse(r)).ToList();
            RemoveUnitRules(rules);
            var count = 0;
            for (int i = 0; i < messageLines.Length; i++)
            {
                var message = messageLines[i];
                Console.WriteLine($"{i}/{messageLines.Length}");

                if (Matches(rules, message))
                {
                    count++;
                }
            }

            Console.WriteLine(count);
        }

        static void RemoveUnitRules(IList<Rule> rules)
        {
            var unitRules = rules.Where(r => r is GrammarRule).Cast<GrammarRule>().Where(r => r.Right.Count == 1).ToList();
            foreach (var rule in unitRules)
            {
                var right = rule.Right.Single();
                var rulesToCopy = rules.Where(r => r.Left == right).ToList();
                foreach (var r in rulesToCopy)
                {
                    switch (r)
                    {
                        case TerminalRule t:
                            rules.Add(new TerminalRule(t.Value, rule.Left));
                            break;
                        case GrammarRule g:
                            rules.Add(new GrammarRule(g.Right, rule.Left));
                            break;
                    }
                    Console.WriteLine(rules.Last());
                    rules.Remove(rule);
                }
            }
        }

        static bool Matches(IEnumerable<Rule> rules, string word)
        {

            Console.WriteLine($"Word: {word}");
            var ruleCount = rules.Count();
            var wordLength = word.Length;
            bool[,,] P = new bool[wordLength + 1, wordLength + 1, ruleCount];

            var terminalRules = rules.Where(r => r is TerminalRule).Cast<TerminalRule>();
            for (int s = 1; s <= wordLength; s++)
            {
                char c = word[s - 1];
                for (int v = 0; v < ruleCount; v++)
                {
                    foreach (var rule in terminalRules.Where(t => t.Value == c))
                    {
                        P[1, s, rule.Left] = true;
                    }
                }
            }

            for (int l = 2; l <= wordLength; l++)
            {
                for (int s = 1; s <= wordLength - l + 1; s++)
                {
                    for (int p = 1; p < l; p++)
                    {
                        foreach (var rule in rules.Where(r => r is GrammarRule).Cast<GrammarRule>())
                        {
                            var a = rule.Left;

                            if (rule.Right.Count < 2)
                            {
                                Console.WriteLine(rule.ToString());
                            }
                            var b = rule.Right[0];
                            var c = rule.Right[1];

                            if (P[p, s, b] && P[l - p, s + p, c])
                            {
                                P[l, s, a] = true;
                            }
                        }
                    }
                }
            }

            return P[wordLength, 1, 0];
        }
    }

    abstract class Rule
    {
        public int Left { get; }

        public Rule(int left)
        {
            Left = left;
        }

        public static IEnumerable<Rule> Parse(string input)
        {
            var inputSplit = input.Split(": ");
            var left = int.Parse(inputSplit[0]);

            var rest = inputSplit[1];
            if (rest.Contains("\""))
            {
                yield return new TerminalRule(rest[1], left);
            }
            else
            {
                var alternatives = rest.Split(" | ").Select(a => a.Split(" ").Select(int.Parse));
                foreach (var alternative in alternatives)
                {
                    yield return new GrammarRule(alternative, left);
                }
            }
        }
    }


    class TerminalRule : Rule
    {
        public TerminalRule(char value, int left) : base(left)
        {
            Value = value;
        }

        public char Value { get; }

        public override string ToString()
        {
            return $"{Left} -> {Value}";
        }
    }


    class GrammarRule : Rule
    {
        public IList<int> Right { get; }
        public GrammarRule(IEnumerable<int> right, int left) : base(left)
        {
            Right = right.ToList();
        }

        public override string ToString()
        {
            return $"{Left} -> {string.Join(" ", Right)}";
        }
    }
}
