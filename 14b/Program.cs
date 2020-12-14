using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _14b
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
                    var index = long.Parse(indexString.Substring(4, indexString.Length - 5));

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
        private IDictionary<long, long> _memory;

        public MemorySystem()
        {
            Mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            _memory = new Dictionary<long, long>();
        }

        public void AddToMemory(long index, long value)
        {
            // var binaryValue = Convert.ToString(value, 2).PadLeft(36, '0');
            // var binaryResult = ApplyMask(binaryValue);
            // _memory[index] = Convert.ToInt64(binaryResult, 2);
            var binaryIndex = Convert.ToString(index, 2).PadLeft(36, '0');
            var indexAfterMask = ApplyMask(binaryIndex);
            foreach(var ind in GenerateAllIndices(indexAfterMask))
            {
                var indexLon = Convert.ToInt64(ind, 2);
                _memory[indexLon] = value;
            }   

        }

        private IEnumerable<string> GenerateAllIndices(string binaryIndex)
        {
            var firstFloatingBit = binaryIndex.IndexOf('X');
            if(firstFloatingBit == -1)
            {
                return new List<string>() { binaryIndex };
            }
            var withZeroSb = new StringBuilder(binaryIndex);
            withZeroSb[firstFloatingBit] = '0';
            var withZero = withZeroSb.ToString();

            var withOneSb = new StringBuilder(binaryIndex);
            withOneSb[firstFloatingBit] = '1';
            var withOne = withOneSb.ToString();

            return GenerateAllIndices(withZero).Concat(GenerateAllIndices(withOne));           
        }

        private string ApplyMask(string binaryValue)
        {
            if (binaryValue.Length != Mask.Length)
                throw new Exception($"Wrong value ({binaryValue.Length}) or mask ({Mask.Length}) length!");
            return new string(binaryValue.Zip(Mask).Select(x => x.Second == '0' ? x.First : x.Second).ToArray());
        }

        public long GetValuesSum()
        {
            return _memory.Sum(x => x.Value);
        }
    }
}
