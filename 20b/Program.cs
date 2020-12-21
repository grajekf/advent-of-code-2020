using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace _20b
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("input.txt");
            var tileStrings = inputString.Split("\r\n\r\n");

            var tiles = tileStrings.Select(Tile.Parse).ToList();

            var solutionMap = GetSolutionMap(tiles);


        }

        static Tile[,] GetSolutionMap(IEnumerable<Tile> tiles)
        {
            var borderDict = tiles
                .SelectMany(t => t.GetPossibleBorders().Select(b => (Tile: t, Border: b)))
                .GroupBy(p => p.Border)
                .ToDictionary(g => g.Key, g => g.ToList());

            var unusedTiles = tiles.Select(t => t.Id).ToHashSet();
            var cornerTiles = tiles.Where(t => t.GetPossibleBorders().Count(b => borderDict[b].Count == 1) > 2).ToList();

            var topLeftTile = cornerTiles[3];
            while (borderDict[topLeftTile.GetBorder(BorderDirection.Left)].Count != 1
             || borderDict[topLeftTile.GetBorder(BorderDirection.Top)].Count != 1)
            {
                topLeftTile = topLeftTile.RotateClockwise();
            }
            unusedTiles.Remove(topLeftTile.Id);

            // Console.WriteLine(topLeftTile);

            int solutionMapSize = (int)Math.Sqrt(tiles.Count());
            Tile[,] solutionMap = new Tile[solutionMapSize, solutionMapSize];
            solutionMap[0, 0] = topLeftTile;

            for (int i = 0; i < solutionMapSize; i++)
            {
                for (int j = 0; j < solutionMapSize; j++)
                {
                    if (solutionMap[i, j] != null)
                        continue;

                    IList<(string Border, BorderDirection Direction)> bordersToFind
                        = new List<(string Border, BorderDirection Direction)>();

                    string leftBorderToFind = null;
                    string topBorderToFind = null;

                    if (i != 0)
                    {
                        var tileAbove = solutionMap[i - 1, j];
                        topBorderToFind = tileAbove.GetBorder(BorderDirection.Bottom);
                    }
                    if (j != 0)
                    {
                        var tileLeft = solutionMap[i, j - 1];
                        leftBorderToFind = tileLeft.GetBorder(BorderDirection.Right);
                    }
                    var possibleNextTiles = new List<Tile>();

                    if (leftBorderToFind != null)
                    {
                        possibleNextTiles.AddRange(borderDict[leftBorderToFind].Select(x => x.Tile));
                    }
                    if (topBorderToFind != null)
                    {
                        possibleNextTiles.AddRange(borderDict[topBorderToFind].Select(x => x.Tile));
                    }
                    var nextTile = possibleNextTiles.First(t => unusedTiles.Contains(t.Id));
                    nextTile = RotateUntilMatches(nextTile, leftBorderToFind, topBorderToFind);
                    // nextTile = possibleAfterTransform.Single();

                    // Console.WriteLine("Next tile after transformations:");
                    // Console.WriteLine(nextTile);

                    solutionMap[i, j] = nextTile;
                    unusedTiles.Remove(nextTile.Id);
                }
            }

            return solutionMap;
        }

        static Tile RotateUntilMatches(Tile tile, string leftBorderToFind, string topBorderToFind)
        {
            for (int t = 0; t <= 1; t++)
            {
                for (int h = 0; h <= 1; h++)
                {
                    for (int v = 0; v <= 1; v++)
                    {
                        var afterTransformations = tile;
                        if (t > 0)
                        {
                            afterTransformations = afterTransformations.Transpose();
                        }
                        if (h > 0)
                        {
                            afterTransformations = afterTransformations.FlipHorizontal();
                        }
                        if (v > 0)
                        {
                            afterTransformations = afterTransformations.FlipVertical();
                        }

                        if ((leftBorderToFind == null
                                || afterTransformations.GetBorder(BorderDirection.Left) == leftBorderToFind)
                            && (topBorderToFind == null
                                || afterTransformations.GetBorder(BorderDirection.Top) == topBorderToFind))
                        {
                            return afterTransformations;
                        }
                    }
                }
            }

            throw new Exception("No transformation found...");
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

        public IEnumerable<string> GetCurrentBorders()
        {
            //top
            yield return new string(Image[0]);

            //right
            yield return new string(Image.Select(row => row.Last()).ToArray());

            //bottom
            yield return new string(Image.Last());

            //left
            yield return new string(Image.Select(row => row.First()).ToArray());
        }

        public string GetBorder(BorderDirection direction)
        {
            var borders = GetPossibleBorders().ToArray();
            return borders[(int)direction];
        }

        public Tile FlipVertical()
        {
            return new Tile(Id, Image.Reverse().Select(row => row.ToArray()).ToArray());
        }

        public Tile FlipHorizontal()
        {
            return new Tile(Id, Image.Select(row => row.Reverse().ToArray()).ToArray());
        }

        public Tile RotateClockwise()
        {
            // var newImage = new char[Image.Length][];

            // for (int i = 0; i < Image.Length; i++)
            // {
            //     newImage[i] = new char[Image[i].Length];
            //     for (int j = 0; j < Image[i].Length; j++)
            //     {
            //         newImage[i][j] = Image[j][i];
            //     }
            // }

            // return new Tile(Id, newImage).FlipHorizontal();

            return this.Transpose().FlipHorizontal();
        }

        public Tile Transpose()
        {
            var newImage = new char[Image.Length][];

            for (int i = 0; i < Image.Length; i++)
            {
                newImage[i] = new char[Image[i].Length];
                for (int j = 0; j < Image[i].Length; j++)
                {
                    newImage[i][j] = Image[j][i];
                }
            }

            return new Tile(Id, newImage);
        }

    }

    enum BorderDirection
    {
        Top = 0,
        TopReverse = 1,
        Right = 2,
        RightReverse = 3,
        Bottom = 4,
        BottomReverse = 5,
        Left = 6,
        LeftReverse = 7,
    }
}
