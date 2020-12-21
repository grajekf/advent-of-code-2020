using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace _20b
{
    class Program
    {

        static private string Seamonster =
@"                  # 
#    ##    ##    ###
 #  #  #  #  #  #   ";
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("input.txt");
            var tileStrings = inputString.Split("\r\n\r\n");

            var tiles = tileStrings.Select(Tile.Parse).ToList();

            var solutionMap = GetSolutionMap(tiles);
            var fullImage = ToFullImage(solutionMap);

            var seamonsters = GetSeamonsterPositions(fullImage).ToList();
            var seaMonsterHashCount = 15;

            Console.WriteLine(GetWaveCount(fullImage, seamonsters.Count, seaMonsterHashCount));
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

        static char[,] ToFullImage(Tile[,] map)
        {
            var mapSize = map.GetLength(0);
            var tileSizeWithoutBorders = map[0, 0].Image.Length - 2;
            var resultSize = mapSize * tileSizeWithoutBorders;

            var result = new char[resultSize, resultSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    var x = j * tileSizeWithoutBorders;
                    var y = i * tileSizeWithoutBorders;

                    var imageToPaste = map[i, j].GetImageWithoutBorders();
                    Paste(result, imageToPaste, x, y);
                }
            }

            return result;
        }

        static IEnumerable<(int x, int y)> GetSeamonsterPositions(char[,] image)
        {
            var seamonsterLines = Seamonster.Replace(" ", ".").Split("\r\n");
            var seamonsterWidth = seamonsterLines[0].Length;
            var firstRowRegex = new Regex(seamonsterLines[0]);
            var middleRowRegex = new Regex(seamonsterLines[1]);
            var lastRowRegex = new Regex(seamonsterLines[2]);

            var imageAsStrings = new List<string>();

            for (int i = 0; i < image.GetLength(0); i++)
            {
                imageAsStrings.Add(ExtractString(image, 0, i, image.GetLength(1)));
            }

            for (int i = 1; i < image.GetLength(0); i++)
            {
                var row = imageAsStrings[i];
                foreach (Match match in middleRowRegex.Matches(row))
                {
                    var x = match.Index;

                    var above = ExtractString(image, x, i - 1, seamonsterWidth);
                    if (!firstRowRegex.IsMatch(above))
                        continue;

                    var below = ExtractString(image, x, i + 1, seamonsterWidth);
                    if (!lastRowRegex.IsMatch(below))
                        continue;

                    yield return (x, i - 1);
                }

            }
        }

        static int GetWaveCount(char[,] image, int seaMonsterCount, int seaMonsterHashCount)
        {
            int result = 0;

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    if (image[i, j] == '#')
                        result++;
                }
            }

            return result - (seaMonsterCount + 1) * seaMonsterHashCount;
        }

        static string ExtractString(char[,] source, int x, int y, int lenght, int height = 1)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    sb.Append(source[y + i, x + j]);
                }

                if (i != height - 1)
                    sb.AppendLine();
            }

            return sb.ToString();
        }

        static void Paste(char[,] target, char[,] source, int x, int y)
        {
            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    target[y + i, x + j] = source[i, j];
                }
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

        public char[,] GetImageWithoutBorders()
        {
            var imageSize = Image.Length - 2;
            var result = new char[imageSize, imageSize];

            for (int i = 1; i < Image.Length - 1; i++)
            {
                for (int j = 1; j < Image.Length - 1; j++)
                {
                    result[i - 1, j - 1] = Image[i][j];
                }
            }

            return result;
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
