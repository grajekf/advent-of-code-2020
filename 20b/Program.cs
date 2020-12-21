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

            Console.WriteLine(topLeftTile);

            int solutionMapSize = (int)Math.Sqrt(tiles.Count);
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

                    if (leftBorderToFind != null)
                    {
                        Console.WriteLine(leftBorderToFind);
                        Console.WriteLine(string.Join(", ", borderDict[leftBorderToFind].Select(x => x.Tile.Id)));
                    }
                    if (topBorderToFind != null)
                    {
                        Console.WriteLine(topBorderToFind);
                        Console.WriteLine(string.Join(", ", borderDict[topBorderToFind].Select(x => x.Tile.Id)));
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

                    Console.WriteLine("Next tile:");
                    Console.WriteLine(nextTile);


                    nextTile = RotateUntilMatches(nextTile, leftBorderToFind, topBorderToFind);

                    Console.WriteLine("Next tile after transformations:");
                    Console.WriteLine(nextTile);

                    // while ((leftBorderToFind == null
                    //         || (nextTile.GetBorder(BorderDirection.Left) != leftBorderToFind) && (nextTile.GetBorder(BorderDirection.LeftReverse) != leftBorderToFind))
                    //     && (topBorderToFind == null
                    //         || nextTile.GetBorder(BorderDirection.Top) != topBorderToFind) && (nextTile.GetBorder(BorderDirection.TopReverse) != topBorderToFind))
                    // {
                    //     nextTile = nextTile.RotateClockwise();
                    // }

                    // Console.WriteLine("Next tile after rotation:");
                    // Console.WriteLine(nextTile);

                    // if (leftBorderToFind != null && nextTile.GetBorder(BorderDirection.Left) != leftBorderToFind)
                    // {
                    //     nextTile = nextTile.FlipVertical();
                    // }

                    // if (topBorderToFind != null && nextTile.GetBorder(BorderDirection.Top) != topBorderToFind)
                    // {
                    //     nextTile = nextTile.FlipHorizontal();
                    // }

                    // Console.WriteLine("Next tile after flips:");
                    // Console.WriteLine(nextTile);

                    // var matchedBordersCount = nextTile.GetPossibleBorders().Count(b => borderDict[b].Count > 1) / 2;
                    // Console.WriteLine($"Matched borders: {matchedBordersCount}");

                    Console.WriteLine($"Setting position {i},{j}");

                    solutionMap[i, j] = nextTile;
                    unusedTiles.Remove(nextTile.Id);
                }
            }

            Tile RotateUntilMatches(Tile tile, string leftBorderToFind, string topBorderToFind)
            {
                for (int rotateCount = 0; rotateCount <= 3; rotateCount++)
                {
                    for (int h = 0; h <= 1; h++)
                    {
                        for (int v = 0; v <= 1; v++)
                        {
                            var afterTransformations = tile;
                            for (int r = 0; r < rotateCount; r++)
                            {
                                afterTransformations = afterTransformations.RotateClockwise();
                            }
                            if (h > 0)
                            {
                                afterTransformations = afterTransformations.FlipHorizontal();
                            }
                            if (v > 0)
                            {
                                afterTransformations = afterTransformations.FlipVertical();
                            }

                            //TODO: Jakos tak obracac zeby na zewnatrz byly odpowiednie granice

                            if (leftBorderToFind == null
                                    || (afterTransformations.GetBorder(BorderDirection.Left) == leftBorderToFind)
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
            var newImage = new char[Image.Length][];

            for (int i = 0; i < Image.Length; i++)
            {
                newImage[i] = new char[Image[i].Length];
                for (int j = 0; j < Image[i].Length; j++)
                {
                    newImage[i][j] = Image[j][i];
                }
            }

            return new Tile(Id, newImage).FlipHorizontal();
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
