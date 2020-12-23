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
            // var input = "389125467";
            var inputArray = input.Select(c => int.Parse(c.ToString())).ToArray();
            var minValue = inputArray.Min();
            var maxValue = inputArray.Max();
            var inputList = new LinkedList<int>(inputArray);

            for(int i = maxValue + 1; i <= 1000000; i++)
            {
                inputList.AddLast(i);
            }

            var currentCup = inputList.First;

            for (int i = 0; i < 100; i++)
            {

                // foreach (var n in inputList)
                // {
                //     if (n == currentCup.Value)
                //     {
                //         Console.Write($"({n}) ");
                //     }
                //     else
                //     {
                //         Console.Write($"{n} ");
                //     }
                // }
                // Console.WriteLine();

                Stack<LinkedListNode<int>> removed = new Stack<LinkedListNode<int>>();
                for (int r = 0; r < 3; r++)
                {
                    var next = inputList.GetNextCircullar(currentCup);
                    removed.Push(next);
                    inputList.Remove(next);
                }

                var valueToFind = currentCup.Value - 1;
                var destination = inputList.Find(valueToFind);
                while (destination == null)
                {
                    valueToFind--;
                    if (valueToFind < minValue)
                        valueToFind = maxValue;
                    destination = inputList.Find(valueToFind);
                }

                while (removed.Count > 0)
                {
                    var node = removed.Pop();
                    inputList.AddAfter(destination, node);
                }

                currentCup = inputList.GetNextCircullar(currentCup);

            }

            var oneNode = inputList.Find(1);
            var resultIt = inputList.GetNextCircullar(oneNode);

            while (resultIt.Value != 1)
            {
                Console.Write(resultIt.Value);
                resultIt = inputList.GetNextCircullar(resultIt);
            }
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
