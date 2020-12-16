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
            var rules = lines.Take(20).Select(l => Rule.Parse(l)).ToList();
            var ruleCount = rules.Count();
            foreach (var (rule, i) in rules.Zip(Enumerable.Range(0, ruleCount)))
            {
                rule.Id = i;
            }


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
            Console.WriteLine(nearbyTickets.Select(t => t.GetErrorRate(rules)).Sum());

            var validTickets = nearbyTickets.Where(t => t.IsValid(rules)).ToList();
            // foreach (var ticket in validTickets)
            // {
            //     Console.WriteLine(ticket);
            // }


            var unassignedFields = new HashSet<int>(Enumerable.Range(0, ruleCount));
            var unusedRules = new HashSet<int>(Enumerable.Range(0, ruleCount));

            var validityMatrix = new ValidityMatrix(ruleCount);
            validityMatrix.Fill(validTickets, rules);
            validityMatrix.Print();

            bool change = true;
            while (change)
            {
                change = false;
                foreach (var rule in unusedRules)
                {
                    var possibleFields = validityMatrix.ValidFieldsForRule(rule).ToList();
                    if (possibleFields.Count == 1)
                    {
                        unusedRules.Remove(rule);
                        var field = possibleFields.Single();
                        unassignedFields.Remove(field);
                        validityMatrix.Assign(field, rule);
                        change = true;
                    }
                }

                foreach (var field in unassignedFields)
                {
                    var possibleRules = validityMatrix.ValidRuleIdsForField(field).ToList();
                    if (possibleRules.Count == 1)
                    {
                        unassignedFields.Remove(field);
                        var rule = possibleRules.Single();
                        unusedRules.Remove(rule);
                        validityMatrix.Assign(field, rule);
                        change = true;
                    }
                }
            }
            // Console.WriteLine("After preprocessing:");
            // validityMatrix.Print();
            // validityMatrix.PrintAssignment();
            long result = 1;
            foreach (var rule in rules.Where(r => r.Field.StartsWith("departure")))
            {
                var field = validityMatrix.GetAssignedField(rule.Id);
                result *= myTicket.Values[field];
            }

            Console.WriteLine("Result:");
            Console.WriteLine(result);
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
        public int Id { get; set; }
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

        public bool IsValid(int value)
        {
            return Ranges.Any(r => r.IsInside(value));
        }

        public bool IsValidForAll(IEnumerable<int> values)
        {
            return values.All(v => IsValid(v));
        }

    }

    class Ticket
    {
        public Ticket(IList<int> values)
        {
            Values = values;
        }

        public IList<int> Values { get; set; }

        public static Ticket Parse(string inp)
        {
            return new Ticket(inp.Split(",").Select(int.Parse).ToList());
        }

        public override string ToString()
        {
            return string.Join(",", Values.Select(v => v.ToString()));
        }

        public int GetErrorRate(IEnumerable<Rule> rules)
        {
            return Values.Where(v => !rules.Any(r => r.IsValid(v))).Sum();
        }

        public bool IsValid(IEnumerable<Rule> rules)
        {
            return Values.All(v => rules.Any(r => r.IsValid(v)));
        }
    }

    class ValidityMatrix
    {
        private bool[,] _matrix;
        private bool[,] _originalMatrix;
        private int _size;

        private IDictionary<int, int> _assignment;
        public ValidityMatrix(int size)
        {
            _size = size;
            _matrix = new bool[_size, _size];
            _assignment = new Dictionary<int, int>();
        }

        public bool RuleValidForField(Rule rule, int field)
        {
            return _matrix[field, rule.Id];
        }

        public IEnumerable<int> ValidRuleIdsForField(int field)
        {
            for (int ruleId = 0; ruleId < _size; ruleId++)
            {
                if (_matrix[field, ruleId])
                    yield return ruleId;
            }
        }

        public IEnumerable<int> ValidFieldsForRule(int ruleId)
        {
            for (int field = 0; field < _size; field++)
            {
                if (_matrix[field, ruleId])
                    yield return field;
            }
        }

        public void Fill(IEnumerable<Ticket> validTickets, IEnumerable<Rule> rules)
        {
            for (int i = 0; i < _size; i++)
            {
                var values = validTickets.Select(v => v.Values[i]);
                var possibleRules = rules.Where(r => r.IsValidForAll(values));

                foreach (var rule in possibleRules)
                {
                    _matrix[i, rule.Id] = true;
                }
            }
            _originalMatrix = _matrix.Clone() as bool[,];
        }

        public void Assign(int field, int ruleId)
        {
            if (!_matrix[field, ruleId])
            {
                throw new Exception("Assignment is not valid!");
            }

            _assignment[field] = ruleId;

            for (int i = 0; i < _size; i++)
            {
                _matrix[field, i] = false;
                _matrix[i, ruleId] = false;
            }
        }

        public void Print()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Console.Write(_matrix[i, j] ? "1" : "0");
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public void PrintAssignment()
        {
            foreach (var (key, value) in _assignment)
            {
                Console.WriteLine($"{key} - {value}");
            }
        }

        public int GetAssignedField(int rule)
        {
            return _assignment.First(a => a.Value == rule).Key;
        }


    }
}
