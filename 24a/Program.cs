using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace _24a
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            var instructions = lines.Select(l => Parse(l)).ToList();
            var blackTiles = new HashSet<(int X, int Y)>();

            foreach (var i in instructions)
            {
                var coords = InstructionsToCoordinate(i);
                if (blackTiles.Contains(coords))
                {
                    blackTiles.Remove(coords);
                }
                else
                {
                    blackTiles.Add(coords);
                }
            }

            Console.WriteLine(blackTiles.Count);

        }

        static IEnumerable<string> Parse(string input)
        {
            string buffer = string.Empty;
            foreach (var c in input)
            {
                if (string.IsNullOrEmpty(buffer))
                {
                    if (c == 'w' || c == 'e')
                    {
                        yield return c.ToString();
                    }
                    else
                    {
                        buffer = c.ToString();
                    }
                }
                else
                {
                    yield return buffer + c.ToString();
                    buffer = string.Empty;
                }
            }
        }

        static (int X, int Y) InstructionsToCoordinate(IEnumerable<string> instructions)
        {
            int x = 0;
            int y = 0;

            foreach (var instruction in instructions)
            {
                switch (instruction)
                {
                    case "e":
                        x += 2;
                        break;
                    case "se":
                        x += 1;
                        y -= 1;
                        break;
                    case "sw":
                        x -= 1;
                        y -= 1;
                        break;
                    case "w":
                        x -= 2;
                        break;
                    case "ne":
                        x += 1;
                        y += 1;
                        break;
                    case "nw":
                        x -= 1;
                        y += 1;
                        break;
                    default:
                        throw new Exception("Unknown instruction");
                }
            }

            return (x, y);
        }
    }
}
