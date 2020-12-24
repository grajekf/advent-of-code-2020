using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace _24b
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

            for (int day = 1; day <= 100; day++)
            {
                var tilesToCheck = new HashSet<(int X, int Y)>(blackTiles);
                var neighbors = blackTiles.SelectMany(t => Neighbors(t.X, t.Y)).ToList();
                tilesToCheck.UnionWith(neighbors);

                var newBlackTiles = new HashSet<(int X, int Y)>();
                foreach (var tile in tilesToCheck)
                {
                    var blackNeighborsCount = Neighbors(tile.X, tile.Y).Count(t => blackTiles.Contains(t));
                    if (blackTiles.Contains(tile))
                    {
                        if (blackNeighborsCount > 0 && blackNeighborsCount <= 2)
                        {
                            newBlackTiles.Add(tile);
                        }
                    }
                    else
                    {
                        if (blackNeighborsCount == 2)
                        {
                            newBlackTiles.Add(tile);
                        }
                    }


                }
                blackTiles = newBlackTiles;
                Console.WriteLine($"Day {day}: {blackTiles.Count}");
            }

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

        static IEnumerable<(int X, int Y)> Neighbors(int x, int y)
        {
            yield return (x + 2, y);
            yield return (x + 1, y - 1);
            yield return (x - 1, y - 1);
            yield return (x - 2, y);
            yield return (x + 1, y + 1);
            yield return (x - 1, y + 1);
        }
    }
}
