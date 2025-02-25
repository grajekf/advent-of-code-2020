﻿using System;
using System.Linq;

namespace _12
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputs = new string[]
            {
               "S2", "L90", "S2", "F13", "L180", "F32", "R90", "F29", "L90", "S3", "W3", "E5", "F25", "N4", "E3", "F35", "W1", "S4", "F32", "E1", "N2", "L180", "N2", "L90", "E4", "S5", "L90", "N2", "R90", "S2", "E3", "N1", "W1", "R90", "F1", "S3", "W2", "F20", "E2", "F34", "E2", "F63", "E4", "F40", "L90", "E4", "R90", "E4", "S1", "F84", "S4", "L180", "N2", "F93", "S1", "E3", "R180", "S4", "W5", "S4", "W1", "S4", "F85", "R180", "W4", "N3", "F29", "S5", "F24", "W3", "F74", "E2", "S3", "E2", "S1", "W4", "L90", "S1", "R90", "E2", "S1", "E4", "N5", "E5", "N2", "F23", "R180", "F43", "N5", "W3", "S1", "L90", "F30", "E1", "R90", "N4", "F94", "N3", "R90", "W1", "F38", "W5", "F76", "N1", "W5", "F31", "E2", "S3", "R180", "F95", "R90", "E4", "S5", "F12", "R90", "F83", "F68", "L90", "N2", "F6", "R90", "E5", "F17", "R90", "F69", "S3", "S3", "E1", "R90", "E2", "F23", "L90", "F17", "R90", "F15", "S2", "L90", "N4", "S5", "L90", "F84", "W1", "F2", "W4", "S5", "F30", "L90", "S4", "R90", "R90", "N2", "L90", "S4", "F47", "W1", "N1", "E4", "L90", "W2", "F85", "E2", "N5", "F32", "L180", "S2", "F20", "E2", "L180", "F2", "W4", "F42", "S1", "R90", "F47", "L90", "F10", "N4", "F57", "R90", "E5", "N4", "F48", "E4", "W1", "N5", "L180", "N4", "E3", "F64", "E4", "F30", "R90", "E3", "R90", "E1", "F100", "N1", "F29", "W5", "R90", "F18", "R90", "W4", "L90", "F36", "W2", "F87", "E2", "R90", "N2", "F31", "N4", "F60", "S3", "F26", "L90", "F60", "R180", "N1", "R90", "L90", "N4", "F82", "S1", "R90", "F26", "R90", "N1", "F31", "N5", "W2", "F74", "R90", "F2", "S1", "E1", "F13", "L90", "N5", "F37", "N1", "R90", "E3", "F46", "R270", "E1", "F35", "W2", "S1", "F86", "W1", "L90", "E4", "N1", "R90", "E4", "S1", "F31", "R90", "S4", "R90", "F14", "W3", "S4", "E4", "F87", "L90", "S3", "W1", "R270", "F49", "L90", "F60", "W3", "F32", "N2", "E5", "L90", "F74", "W1", "F43", "W4", "F77", "E4", "R270", "F100", "W3", "S2", "R90", "E1", "F26", "E5", "N4", "F7", "N1", "L90", "F49", "S1", "E1", "N1", "N1", "W2", "F35", "L90", "F29", "R90", "W1", "S4", "F41", "S4", "L270", "N1", "E3", "R180", "F63", "R90", "S3", "R90", "F86", "N2", "S4", "F37", "N1", "F94", "F46", "R270", "S1", "E4", "F66", "W1", "N4", "R90", "F66", "L180", "N5", "R90", "R90", "S5", "F51", "R90", "N5", "F93", "S2", "L90", "S5", "W4", "F45", "E1", "S2", "F99", "W2", "F7", "E3", "S1", "R180", "E2", "F60", "L90", "F64", "E4", "N3", "F79", "L90", "W1", "F52", "L90", "E3", "R90", "W3", "S3", "F7", "R180", "S4", "W1", "R90", "N5", "R90", "F67", "E2", "F87", "W2", "F24", "N1", "E3", "F73", "S3", "W2", "N3", "R90", "F45", "S5", "F14", "S1", "F53", "S5", "W2", "R180", "F17", "W2", "L90", "F25", "L180", "W1", "L90", "F53", "S2", "R90", "F90", "E4", "F42", "R90", "S1", "F91", "W4", "R90", "F3", "N5", "F69", "F91", "S3", "W4", "S1", "W1", "L90", "N4", "L90", "W3", "L180", "F9", "E3", "L180", "N5", "F84", "L180", "F19", "L90", "S1", "W3", "L180", "F66", "R180", "S1", "W5", "L90", "N5", "R180", "W1", "R180", "F81", "S4", "L90", "F76", "W2", "N3", "L90", "F31", "S4", "E5", "R90", "N3", "F65", "W3", "R180", "W3", "F56", "N4", "W5", "E3", "R90", "E2", "R90", "F56", "S5", "R90", "N2", "F24", "R90", "F16", "N4", "E5", "L90", "S5", "R90", "S2", "W3", "L90", "S4", "F48", "W5", "R90", "L90", "R180", "E1", "S2", "F65", "E5", "N3", "W1", "L180", "W3", "F43", "F80", "E4", "F22", "N4", "E3", "F41", "N2", "E3", "F62", "E4", "F83", "N3", "E1", "W4", "F64", "R90", "F50", "W3", "F79", "L90", "S1", "F36", "R90", "N5", "R270", "F65", "E3", "F68", "W5", "L270", "E3", "S4", "F41", "L90", "W5", "R90", "E2", "S2", "R90", "F79", "E3", "N4", "W4", "F32", "L90", "N4", "F64", "R90", "W5", "R90", "E3", "F84", "L180", "S5", "E5", "R180", "E1", "S4", "F36", "R90", "F51", "N2", "W1", "N1", "W1", "S5", "E1", "F61", "W2", "S3", "R270", "E2", "N5", "W2", "F65", "S1", "E3", "S1", "F19", "N2", "F46", "S2", "L180", "N5", "W1", "F41", "R180", "S3", "F58", "S3", "F74", "S5", "W5", "S1", "W5", "F81", "L90", "N2", "L90", "E2", "F59", "W2", "R90", "F34", "N5", "F90", "F56", "S4", "R180", "F75", "E5", "N3", "F88", "R180", "F76", "R90", "F83", "N1", "R90", "E3", "R270", "E1", "F83", "E5", "L90", "W3", "N2", "F88", "F98", "L90", "S4", "E3", "R90", "E1", "R90", "W5", "S2", "F3", "L90", "N5", "L180", "E4", "F65", "S3", "L90", "E5", "N3", "L90", "F84", "S5", "F31", "N5", "L90", "F42", "S5", "R180", "E2", "L90", "F81", "L180", "W3", "R90", "W5", "S2", "E5", "L90", "F38", "L90", "W5", "R90", "F46", "N3", "L90", "F45", "E4", "F32", "N4", "F90", "E1", "F46", "E2", "S4", "L90", "N3", "E2", "F42", "W4", "F67", "S2", "L90", "S3", "L90", "F1", "W2", "L90", "W1", "S5", "F82", "L90", "F26", "W2", "F95", "N4", "R180", "F100", "S2", "F7", "L90", "W5", "S2", "F31", "R90", "W4", "R90", "E4", "F11", "N4", "R90", "E3", "R270", "F65", "E3", "N5", "F13", "W3", "S5", "R90", "S4", "F62", "L90", "F26", "L90", "F9", "S4", "F8", "R90", "E1", "L90", "F11", "R90", "F57", "R270", "E4", "L180", "S3", "E5", "F43", "S5", "F35", "R90", "S1", "F61", "W4", "N3", "F77", "S1", "W1", "L180", "E5", "S4", "L90", "F53", "W3", "F80", "E2", "F69", "R270", "E2", "L90", "S1", "R90", "F67", "L90", "N2", "F45", "W3", "R90", "E2", "R90", "F29", "L90", "E1", "R180", "F80", "E4", "S5", "R90", "S2", "F54"
            };

            var state = ShipState.Initial;

            foreach (var input in inputs)
            {
                var instruction = Instruction.Parse(input);
                state = instruction.Execute(state);
            }

            Console.WriteLine(state.Position);
            Console.WriteLine(state.Direction);
            Console.WriteLine(Math.Abs(state.Position.X) + Math.Abs(state.Position.Y));

        }
    }

    class ShipState
    {
        public ShipState((int X, int Y) position, (int DX, int DY) direction)
        {
            Position = position;
            Direction = direction;
        }

        public (int X, int Y) Position { get; set; }
        public (int DX, int DY) Direction { get; set; }

        public static ShipState Initial => new ShipState((0, 0), (1, 0));

        public override string ToString()
        {
            return $"Pos: {Position}, Dir: {Direction}";
        }
    }

    class Instruction
    {
        public Instruction(char direction, int parameter)
        {
            Direction = direction;
            Parameter = parameter;
        }

        public char Direction { get; set; }
        public int Parameter { get; set; }

        public static Instruction Parse(string input)
        {
            char instr = input[0];
            int param = int.Parse(input.Substring(1));

            return new Instruction(instr, param);
        }

        private (int DX, int DY)[] _possibleDirection = new (int DX, int DY)[]
        {
            (1, 0),
            (0, -1),
            (-1, 0),
            (0, 1)
        };

        public ShipState Execute(ShipState state)
        {
            if (Direction == 'L' || Direction == 'R')
            {
                var curDirectionIndex = Array.IndexOf(_possibleDirection, state.Direction);
                var searchDirection = Direction == 'L' ? -1 : 1;
                var searchSteps = Parameter / 90;
                var newIndex = (curDirectionIndex + searchDirection * searchSteps) % _possibleDirection.Length;

                if (newIndex < 0)
                    newIndex += _possibleDirection.Length;

                var newDirection = _possibleDirection[newIndex];

                return new ShipState(state.Position, newDirection);
            }

            var moveDirection = Direction switch
            {
                'F' => state.Direction,
                'N' => (DX: 0, DY: 1),
                'S' => (DX: 0, DY: -1),
                'E' => (DX: 1, DY: 0),
                'W' => (DX: -1, DY: 0),
                _ => throw new Exception("Unexpected Instruction")
            };

            var newX = state.Position.X + moveDirection.DX * Parameter;
            var newY = state.Position.Y + moveDirection.DY * Parameter;

            return new ShipState((newX, newY), state.Direction);
        }

        public override string ToString()
        {
            return Direction.ToString() + Parameter.ToString();
        }

    }
}
