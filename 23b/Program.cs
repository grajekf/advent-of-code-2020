using System;
using System.Collections.Generic;
using System.Linq;

namespace _23b
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "364289715";
            var inputArray = input.Select(c => int.Parse(c.ToString())).ToArray();
            var minValue = inputArray.Min();
            var maxValue = inputArray.Max();
            var inputList = new LinkedList<int>(inputArray);

            for (int i = maxValue + 1; i <= 1_000_000; i++)
            {
                inputList.AddLast(i);
                if (i > maxValue)
                    maxValue = i;
            }

            var nodeDictionary = new LinkedListNode<int>[inputList.Count + 1];

            var it = inputList.First;
            while (it != null)
            {
                nodeDictionary[it.Value] = it;
                it = it.Next;
            }

            var currentCup = inputList.First;

            for (int i = 0; i < 10000000; i++)
            {
                Stack<LinkedListNode<int>> removed = new Stack<LinkedListNode<int>>();
                for (int r = 0; r < 3; r++)
                {
                    var next = inputList.GetNextCircullar(currentCup);
                    removed.Push(next);
                    inputList.Remove(next);
                }

                var valueToFind = currentCup.Value - 1;
                if (valueToFind < minValue)
                    valueToFind = maxValue;
                var destination = nodeDictionary[valueToFind];
                while (removed.Any(r => r.Value == destination.Value))
                {
                    valueToFind--;
                    if (valueToFind < minValue)
                        valueToFind = maxValue;
                    destination = nodeDictionary[valueToFind];
                    if (destination == null)
                        Console.WriteLine(valueToFind);
                }

                while (removed.Count > 0)
                {
                    var node = removed.Pop();
                    inputList.AddAfter(destination, node);
                }

                currentCup = inputList.GetNextCircullar(currentCup);

            }

            var oneNode = inputList.Find(1);
            var firstAfter = inputList.GetNextCircullar(oneNode);
            var secondAfter = inputList.GetNextCircullar(firstAfter);

            long result = (long)firstAfter.Value * (long)secondAfter.Value;
            Console.WriteLine(result);
        }


    }

    static class LinkedListExtensions
    {
        public static void RemoveAfterCircular<T>(this LinkedList<T> list, LinkedListNode<T> node)
        {
            if (node == list.Last)
                list.RemoveFirst();
            else
            {
                list.Remove(node.Next);
            }
        }

        public static LinkedListNode<T> GetNextCircullar<T>(this LinkedList<T> list, LinkedListNode<T> node)
        {
            if (node == list.Last)
                return list.First;
            return node.Next;
        }
    }
}
