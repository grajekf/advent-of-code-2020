using System;
using System.Linq;
using System.Collections.Generic;

namespace _17b
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = @"..##.##.
            #.#..###
            ##.#.#.#
            #.#.##.#
            ###..#..
            .#.#..##
            #.##.###
            #.#..##.";
            //     var input = @".#.
            // ..#
            // ###";
            var automata = new ThreeDimensionalAutomata(input);
            for (int i = 0; i < 6; i++)
            {
                automata.Step();
            }

            Console.WriteLine(automata.ActiveCount());
        }
    }

    class ThreeDimensionalAutomata
    {
        private IDictionary<(int x, int y, int z, int w), bool> _board;

        private (int min, int max) _xBounds;
        private (int min, int max) _yBounds;
        private (int min, int max) _zBounds;
        private (int min, int max) _wBounds;
        public ThreeDimensionalAutomata(string initialState)
        {
            _board = ParseLayer(initialState);
            _xBounds = (_board.Keys.Select(k => k.x).Min(), _board.Keys.Select(k => k.x).Max());
            _yBounds = (_board.Keys.Select(k => k.y).Min(), _board.Keys.Select(k => k.y).Max());
            _zBounds = (_board.Keys.Select(k => k.z).Min(), _board.Keys.Select(k => k.z).Max());
            _wBounds = (_board.Keys.Select(k => k.w).Min(), _board.Keys.Select(k => k.w).Max());
        }

        private static IDictionary<(int x, int y, int z, int w), bool> ParseLayer(string layer)
        {
            var z = 0;
            var w = 0;
            var result = new Dictionary<(int x, int y, int z, int w), bool>();
            var rows = layer.Split("\r\n");

            for (int j = 0; j < rows.Length; j++)
            {
                var row = rows[j].Trim();
                for (int i = 0; i < row.Length; i++)
                {
                    var element = row[i];
                    if (element == '#')
                        result.Add((i, j, z, w), true);
                    if (element == '.')
                        result.Add((i, j, z, w), false);
                }
            }

            return result;
        }

        public void Step()
        {
            var newBoard = new Dictionary<(int x, int y, int z, int w), bool>();

            var xFrom = _xBounds.min - 1;
            var xTo = _xBounds.max + 1;
            var yFrom = _yBounds.min - 1;
            var yTo = _yBounds.max + 1;
            var zFrom = _zBounds.min - 1;
            var zTo = _zBounds.max + 1;
            var wFrom = _wBounds.min - 1;
            var wTo = _wBounds.max + 1;


            for (int i = xFrom; i <= xTo; i++)
            {
                for (int j = yFrom; j <= yTo; j++)
                {
                    for (int k = zFrom; k <= zTo; k++)
                    {
                        for (int l = wFrom; l <= wTo; l++)
                        {
                            var neighbours = Neighbours(i, j, k, l).ToList();
                            var activeCount = neighbours.Count(n => n == true);
                            var value = ValueAt(i, j, k, l);
                            // if (i == 1 && j == 2 && k == 0)
                            // {
                            //     Console.WriteLine(value);
                            //     Console.WriteLine(activeCount);
                            // }
                            if (value)
                            {
                                if (activeCount != 2 && activeCount != 3)
                                {
                                    SetValue(newBoard, i, j, k, l, false);
                                }
                                else
                                {
                                    SetValue(newBoard, i, j, k, l, true);
                                }
                            }
                            else
                            {
                                if (activeCount == 3)
                                {
                                    SetValue(newBoard, i, j, k, l, true);
                                }
                            }
                        }
                    }
                }
            }

            _board = newBoard;

        }

        private void SetValue(IDictionary<(int x, int y, int z, int w), bool> newBoard, int x, int y, int z, int w, bool value)
        {

            newBoard[(x, y, z, w)] = value;
            if (x < _xBounds.min)
            {
                _xBounds.min = x;
            }
            if (x > _xBounds.max)
            {
                _xBounds.max = x;
            }
            if (y < _yBounds.min)
            {
                _yBounds.min = y;
            }
            if (y > _yBounds.max)
            {
                _yBounds.max = y;
            }
            if (z < _zBounds.min)
            {
                _zBounds.min = z;
            }
            if (z > _zBounds.max)
            {
                _zBounds.max = z;
            }
            if (w < _wBounds.min)
            {
                _wBounds.min = w;
            }
            if (w > _wBounds.max)
            {
                _wBounds.max = w;
            }
        }

        private IEnumerable<bool> Neighbours(int x, int y, int z, int w)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    for (int k = z - 1; k <= z + 1; k++)
                    {
                        for (int l = w - 1; l <= w + 1; l++)
                        {
                            if (i == x && j == y && k == z && l == w)
                                continue;

                            // if (x == 2 && y == 0 && z == 0 && k == z)
                            // {
                            //     Console.WriteLine((i, j, k));
                            //     Console.WriteLine(ValueAt(i, j, k));
                            // }

                            yield return ValueAt(i, j, k, l);
                        }
                    }
                }
            }
        }

        public bool ValueAt(int x, int y, int z, int w)
        {
            if (_board.ContainsKey((x, y, z, w)))
            {
                return _board[(x, y, z, w)];
            }
            else
            {
                return false;
            }
        }

        public int ActiveCount()
        {
            return _board.Count(x => x.Value);
        }
    }
}
