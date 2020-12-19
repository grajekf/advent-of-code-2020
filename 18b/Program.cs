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

            // var test = "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))";
            // var tokens = Tokenize(test).ToList();
            // foreach (var token in tokens)
            // {
            //     Console.WriteLine($"{token.Value} {token.Type.ToString()}");
            // }

            // var tree = ToAbstractSyntaxTree(tokens);
            // tree.Print();
            // Console.WriteLine(tree.Evaluate());


        }

        public static IEnumerable<Token> Tokenize(string input)
        {
            var token = string.Empty;
            foreach (var c in input)
            {
                if (c == ' ')
                    continue;
                var value = c.ToString();
                var type = GetType(value);

                yield return new Token(value, type);
            }
        }

        public static Node ToAbstractSyntaxTree(IList<Token> tokens)
        {
            Stack<Node> operators = new Stack<Node>();
            Stack<Node> arguments = new Stack<Node>();

            bool afterArgument = false;

            while (tokens.Count > 0)
            {
                var currentToken = tokens.First();
                tokens.RemoveAt(0);

                if (afterArgument)
                {
                    if (currentToken.Type == SymbolType.RParen)
                    {
                        Node op;

                        while (operators.TryPop(out op) && op.Type != SymbolType.LParen)
                        {
                            CreateOperation(op, arguments);
                        }
                        continue;
                    }
                    else
                    {
                        afterArgument = false;
                        var op = currentToken.ToNode();
                        while (operators.Count > 0
                            && operators.Peek().Type != SymbolType.LParen
                            && operators.Peek().Precedence > op.Precedence)
                        {
                            CreateOperation(operators.Pop(), arguments);
                        }
                        operators.Push(op);
                    }
                }
                else
                {
                    if (currentToken.Type == SymbolType.LParen)
                    {
                        operators.Push(currentToken.ToNode());
                    }
                    else
                    {
                        afterArgument = true;
                        arguments.Push(currentToken.ToNode());
                    }
                }
            }

            while (operators.Count > 0)
            {
                Node op = operators.Pop();
                if (op.Type == SymbolType.LParen)
                {
                    throw new Exception("Unexpected left parenthesis");
                }
                CreateOperation(op, arguments);
            }

            var result = arguments.Pop();
            if (arguments.Count > 0)
            {
                throw new Exception("Not empty arguments stack after parsing");
            }

            return result;


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

        private static void CreateOperation(Node op, Stack<Node> arguments)
        {
            var left = arguments.Pop();
            var right = arguments.Pop();

            op.Left = left;
            op.Right = right;
            arguments.Push(op);
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

    class Token
    {
        public Token(string value, SymbolType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; private set; }
        public SymbolType Type { get; private set; }

        public Node ToNode()
        {
            switch (Type)
            {
                case SymbolType.LParen:
                    return new LParenNode();
                case SymbolType.RParen:
                    return new RParenNode();
                case SymbolType.Plus:
                    return new PlusNode();
                case SymbolType.Multiply:
                    return new MultiplyNode();
                case SymbolType.Number:
                    return new NumberNode(long.Parse(Value));
            }

            throw new Exception("Cannot infer Node type");

        }
    }

    abstract class Node
    {
        protected Node(SymbolType type)
        {
            Type = type;
        }

        public Node Left { get; set; }
        public Node Right { get; set; }
        public SymbolType Type { get; set; }

        public abstract int Precedence { get; }

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
        public PlusNode() : base(SymbolType.Plus)
        {
        }

        public override int Precedence => 2;

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
        public MultiplyNode() : base(SymbolType.Multiply)
        {
        }

        public override int Precedence => 1;
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
        public override int Precedence => throw new NotImplementedException();
        public NumberNode(long value) : base(SymbolType.Number)
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

    class LParenNode : Node
    {
        public LParenNode() : base(SymbolType.LParen)
        {
        }

        public override int Precedence => throw new NotImplementedException();

        public override long Evaluate()
        {
            throw new NotImplementedException();
        }

        protected override void PrintSelf()
        {
            Console.WriteLine("(");
        }
    }

    class RParenNode : Node
    {
        public RParenNode() : base(SymbolType.RParen)
        {
        }

        public override int Precedence => throw new NotImplementedException();

        public override long Evaluate()
        {
            throw new NotImplementedException();
        }

        protected override void PrintSelf()
        {
            Console.WriteLine(")");
        }
    }

}
