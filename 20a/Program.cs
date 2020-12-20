using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace _20a
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("input.txt");
            var tileStrings = inputString.Split("\r\n\r\n");

            var tiles = tileStrings.Select(Tile.Parse).ToList();
            // foreach (var tile in tiles)
            // {
            //     Console.WriteLine(tile);
            // }

            var borderDict = tiles
                .SelectMany(t => t.GetPossibleBorders().Select(b => (Tile: t, Border: b)))
                .GroupBy(p => p.Border)
                .ToDictionary(g => g.Key, g => g.ToList());

            var cornerTiles = tiles.Where(t => t.GetPossibleBorders().Count(b => borderDict[b].Count == 1) > 2);
            long result = 1;
            foreach (var tile in cornerTiles)
            {
                result *= tile.Id;
            }
            Console.WriteLine(result);
        }
    }

    class Tile
    {
        public Tile(int id, char[][] image)
        {
            Id = id;
            Image = image;
        }

        public int Id { get; }
        public char[][] Image { get; }

        public static Tile Parse(string input)
        {
            var lines = input.Split("\r\n");
            var idLine = lines[0];
            var imageLines = lines.Skip(1).ToList();

            var id = int.Parse(idLine.Split(" ")[1].Replace(":", ""));

            var image = new char[imageLines.Count][];
            for (int i = 0; i < imageLines.Count; i++)
            {
                image[i] = new char[imageLines.Count];
                for (int j = 0; j < imageLines[i].Length; j++)
                {
                    image[i][j] = imageLines[i][j];
                }
            }

            return new Tile(id, image);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Tile {Id}:");
            for (int i = 0; i < Image.Length; i++)
            {
                for (int j = 0; j < Image[i].Length; j++)
                {
                    sb.Append(Image[i][j]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public IEnumerable<string> GetPossibleBorders()
        {
            //top
            yield return new string(Image[0]);
            yield return new string(Image[0].Reverse().ToArray());

            //right
            yield return new string(Image.Select(row => row.Last()).ToArray());
            yield return new string(Image.Select(row => row.Last()).Reverse().ToArray());

            //bottom
            yield return new string(Image.Last());
            yield return new string(Image.Last().Reverse().ToArray());

            //left
            yield return new string(Image.Select(row => row.First()).ToArray());
            yield return new string(Image.Select(row => row.First()).Reverse().ToArray());
        }


    }
}
