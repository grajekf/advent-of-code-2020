using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace _18b
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = File.ReadAllText("input.txt");

            var lines = inputString.Split("\r\n");
            Console.WriteLine(lines.Select(l =>
            {
                var tokens = Tokenize(l).ToList();
                var tree = ToAbstractSyntaxTree(tokens);
                return tree.Evaluate();
            }).Sum());

            // long sum = 0;
            // foreach (var line in lines)
            // {
            //     var tokens = Tokenize(line);
            //     var tree = ToAbstractSyntaxTree(tokens);
            //     var value = tree.Evaluate();
            //     sum = sum + value;
            //     Console.WriteLine(line);
            //     Console.WriteLine($"Value: {value} Sum: {sum}");
            // }
            // var test = "4 * (3 * (6 + 1) + 2)";
            // var tokens = Tokenize(test).ToList();
            // foreach (var token in tokens)
            // {
            //     Console.WriteLine($"{token} {GetType(token).ToString()}");
            // }

            // var tree = ToAbstractSyntaxTree(tokens);
            // tree.Print();
            // Console.WriteLine(tree.Evaluate());


        }

        public static IEnumerable<string> Tokenize(string input)
        {
            var token = string.Empty;
            foreach (var c in input)
            {
                if (c == ' ')
                    continue;
                yield return c.ToString();
            }
        }

        public static Node ToAbstractSyntaxTree(IList<string> tokens)
        {
            Node current = null;

            // var tokensCopy = tokens.ToList();

            while (tokens.Count > 0)
            {
                var currentToken = tokens.First();
                tokens.RemoveAt(0);
                var tokenType = GetType(currentToken);

                switch (tokenType)
                {
                    case SymbolType.LParen:
                        var insideNode = ToAbstractSyntaxTree(tokens);
                        if (current == null)
                        {
                            current = insideNode;
                        }
                        else
                        {
                            current.Right = insideNode;
                        }
                        break;
                    case SymbolType.RParen:
                        return current;
                    case SymbolType.Number:
                        Node numberNode = new NumberNode(long.Parse(currentToken));
                        if (current == null)
                        {
                            current = numberNode;
                        }
                        else
                        {
                            current.Right = numberNode;
                        }
                        break;
                    case SymbolType.Plus:
                    case SymbolType.Multiply:
                        Node opNode = tokenType == SymbolType.Plus ? new PlusNode() : new MultiplyNode();
                        opNode.Left = current;
                        current = opNode;
                        break;
                }

            }

            return current;
        }

        private static SymbolType GetType(string s)
        {
            if (long.TryParse(s, out _))
                return SymbolType.Number;
            if (s == "(")
                return SymbolType.LParen;
            if (s == ")")
                return SymbolType.RParen;
            if (s == "+")
                return SymbolType.Plus;
            if (s == "*")
                return SymbolType.Multiply;

            throw new Exception("Unrecognized symbol type");
        }
    }

    enum SymbolType
    {
        Number,
        LParen,
        RParen,
        Plus,
        Multiply
    }

    abstract class Node
    {
        public Node Left { get; set; }
        public Node Right { get; set; }

        public abstract long Evaluate();
        public void Print(int level = 0)
        {
            for (int i = 0; i < level; i++)
            {
                Console.Write("\t");
            }
            PrintSelf();
            if (Left != null)
            {
                Left.Print(level + 1);
            }
            if (Right != null)
            {
                Right.Print(level + 1);
            }
        }

        protected abstract void PrintSelf();
    }

    class PlusNode : Node
    {
        public override long Evaluate()
        {
            return Left.Evaluate() + Right.Evaluate();
        }

        protected override void PrintSelf()
        {
            Console.WriteLine("+");
        }
    }

    class MultiplyNode : Node
    {
        public override long Evaluate()
        {
            return Left.Evaluate() * Right.Evaluate();
        }

        protected override void PrintSelf()
        {
            Console.WriteLine("*");
        }
    }

    class NumberNode : Node
    {
        public NumberNode(long value)
        {
            Value = value;
        }

        public long Value { get; set; }

        public override long Evaluate()
        {
            if (Left != null || Right != null)
                throw new Exception("NumberNode cannot have children");

            return Value;
        }

        protected override void PrintSelf()
        {
            Console.WriteLine(Value);
        }
    }

}
