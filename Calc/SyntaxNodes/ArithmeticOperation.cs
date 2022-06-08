using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.SyntaxNodes
{
    public class ArithmeticOperation : SyntaxNode
    {
        public ArithmeticOperation(Operator @operator)
        {
            Operator = @operator;
        }

        public Operator Operator { get; }

        public override void Accept(IVisitor visitor)
        {
            LeftChild?.Accept(visitor);
            RightChild?.Accept(visitor);
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"( {LeftChild} {Operator} {RightChild} )";
        }
    }
}
