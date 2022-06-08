using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.SyntaxNodes
{
    public class Constant : SyntaxNode
    {
        public Constant(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public override SyntaxNode LeftChild 
        { 
            get => null; 
            set => throw new InvalidOperationException(); 
        }

        public override SyntaxNode RightChild
        {
            get => null;
            set => throw new InvalidOperationException();
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
