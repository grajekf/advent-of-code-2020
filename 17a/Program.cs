using System;
using System.Linq;
using System.Collections.Generic;

namespace _17a
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
            automata.Print();
            for (int i = 0; i < 6; i++)
            {
                // Console.WriteLine("==========================================");
                automata.Step();
                // automata.Print();
            }

            Console.WriteLine(automata.ActiveCount());
        }
    }

    class ThreeDimensionalAutomata
    {
        private IDictionary<(int x, int y, int z), bool> _board;

        private (int min, int max) _xBounds;
        private (int min, int max) _yBounds;
        private (int min, int max) _zBounds;
        public ThreeDimensionalAutomata(string initialState)
        {
            _board = ParseLayer(initialState);
            _xBounds = (_board.Keys.Select(k => k.x).Min(), _board.Keys.Select(k => k.x).Max());
            _yBounds = (_board.Keys.Select(k => k.y).Min(), _board.Keys.Select(k => k.y).Max());
            _zBounds = (_board.Keys.Select(k => k.z).Min(), _board.Keys.Select(k => k.z).Max());
        }

        private static IDictionary<(int x, int y, int z), bool> ParseLayer(string layer)
        {
            var z = 0;
            var result = new Dictionary<(int x, int y, int z), bool>();
            var rows = layer.Split("\r\n");

            for (int j = 0; j < rows.Length; j++)
            {
                var row = rows[j].Trim();
                for (int i = 0; i < row.Length; i++)
                {
                    var element = row[i];
                    if (element == '#')
                        result.Add((i, j, z), true);
                    if (element == '.')
                        result.Add((i, j, z), false);
                }
            }

            return result;
        }

        public void Step()
        {
            var newBoard = new Dictionary<(int x, int y, int z), bool>();

            var xFrom = _xBounds.min - 1;
            var xTo = _xBounds.max + 1;
            var yFrom = _yBounds.min - 1;
            var yTo = _yBounds.max + 1;
            var zFrom = _zBounds.min - 1;
            var zTo = _zBounds.max + 1;

            for (int i = xFrom; i <= xTo; i++)
            {
                for (int j = yFrom; j <= yTo; j++)
                {
                    for (int k = zFrom; k <= zTo; k++)
                    {
                        var neighbours = Neighbours(i, j, k).ToList();
                        var activeCount = neighbours.Count(n => n == true);
                        var value = ValueAt(i, j, k);
                        // if (i == 1 && j == 2 && k == 0)
                        // {
                        //     Console.WriteLine(value);
                        //     Console.WriteLine(activeCount);
                        // }
                        if (value)
                        {
                            if (activeCount != 2 && activeCount != 3)
                            {
                                SetValue(newBoard, i, j, k, false);
                            }
                            else
                            {
                                SetValue(newBoard, i, j, k, true);
                            }
                        }
                        else
                        {
                            if (activeCount == 3)
                            {
                                SetValue(newBoard, i, j, k, true);
                            }
                        }
                    }
                }
            }

            _board = newBoard;

        }

        private void SetValue(IDictionary<(int x, int y, int z), bool> newBoard, int x, int y, int z, bool value)
        {

            newBoard[(x, y, z)] = value;
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
        }

        private IEnumerable<bool> Neighbours(int x, int y, int z)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    for (int k = z - 1; k <= z + 1; k++)
                    {
                        if (i == x && j == y && k == z)
                            continue;

                        // if (x == 2 && y == 0 && z == 0 && k == z)
                        // {
                        //     Console.WriteLine((i, j, k));
                        //     Console.WriteLine(ValueAt(i, j, k));
                        // }

                        yield return ValueAt(i, j, k);
                    }
                }
            }
        }

        public bool ValueAt(int x, int y, int z)
        {
            if (_board.ContainsKey((x, y, z)))
            {
                return _board[(x, y, z)];
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

        public void Print()
        {
            for (int k = 0; k <= 0; k++)
            {
                Console.WriteLine($"z = {k}");
                for (int j = _yBounds.min; j <= _yBounds.max; j++)
                {
                    for (int i = _xBounds.min; i <= _xBounds.max; i++)
                    {

                        Console.Write(ValueAt(i, j, k) ? '#' : '.');
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
