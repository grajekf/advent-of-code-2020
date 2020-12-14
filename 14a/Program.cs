using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _14a
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("input.txt");

            var memorySystem = new MemorySystem();

            foreach (var line in inputString.Split("\r\n"))
            {
                if (line.StartsWith("mem"))
                {
                    var lineSplit = line.Split(" = ");
                    var indexString = lineSplit[0];
                    var value = long.Parse(lineSplit[1]);
                    var index = int.Parse(indexString.Substring(4, indexString.Length - 5));

                    memorySystem.AddToMemory(index, value);
                }
                if (line.StartsWith("mask"))
                {
                    var lineSplit = line.Split(" = ");
                    var mask = lineSplit[1];

                    memorySystem.Mask = mask;
                }
            }

            Console.WriteLine(memorySystem.GetValuesSum());
        }
    }

    class MemorySystem
    {
        public string Mask { get; set; }
        private IDictionary<int, long> _memory;

        public MemorySystem()
        {
            Mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            _memory = new Dictionary<int, long>();
        }

        public void AddToMemory(int index, long value)
        {
            var binaryValue = Convert.ToString(value, 2).PadLeft(36, '0');
            var binaryResult = ApplyMask(binaryValue);
            _memory[index] = Convert.ToInt64(binaryResult, 2);

        }

        private string ApplyMask(string binaryValue)
        {
            if (binaryValue.Length != Mask.Length)
                throw new Exception($"Wrong value ({binaryValue.Length}) or mask ({Mask.Length}) length!");
            return new string(binaryValue.Zip(Mask).Select(x => x.Second == 'X' ? x.First : x.Second).ToArray());
        }

        public long GetValuesSum()
        {
            return _memory.Sum(x => x.Value);
        }
    }
}
