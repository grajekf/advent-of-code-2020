using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;

namespace _16a
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("input.txt");

            var lines = inputString.Split("\r\n");
            var rules = lines.Take(20).Select(l => Rule.Parse(l));

            // foreach (var rule in rules)
            // {
            //     Console.WriteLine(rule.ToString());
            // }

            var myTicket = Ticket.Parse(lines[22]);
            var nearbyTickets = lines.Skip(25).Select(Ticket.Parse);

            // Console.WriteLine(myTicket);
            // foreach (var ticket in nearbyTickets)
            // {
            //     Console.WriteLine(ticket);
            // }
        }
    }

    class Range
    {
        public Range(int from, int to)
        {
            From = from;
            To = to;
        }

        public int From { get; set; }
        public int To { get; set; }

        public bool IsInside(int value)
        {
            return value >= From && value <= To;
        }

        public override string ToString()
        {
            return $"{From}-{To}";
        }
    }

    class Rule
    {
        public Rule(string field, IEnumerable<Range> ranges)
        {
            Field = field;
            Ranges = ranges;
        }

        public string Field { get; set; }
        public IEnumerable<Range> Ranges { get; set; }

        public static Rule Parse(string inp)
        {
            var stringSplit = inp.Split(": ");
            var field = stringSplit[0];
            var rangesString = stringSplit[1];
            var ranges = rangesString.Split(" or ").Select(range =>
            {
                var rangeSplit = range.Split("-");
                return new Range(int.Parse(rangeSplit[0]), int.Parse(rangeSplit[1]));
            });

            return new Rule(field, ranges);
        }

        public override string ToString()
        {
            var rangesString = string.Join(" or ", Ranges.Select(r => r.ToString()));
            return $"{Field}: {rangesString}";
        }

    }

    class Ticket
    {
        public Ticket(IEnumerable<int> values)
        {
            Values = values;
        }

        public IEnumerable<int> Values { get; set; }

        public static Ticket Parse(string inp)
        {
            return new Ticket(inp.Split(",").Select(int.Parse));
        }

        public override string ToString()
        {
            return string.Join(",", Values.Select(v => v.ToString()));
        }
    }
}
