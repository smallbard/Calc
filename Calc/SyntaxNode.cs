using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public abstract class SyntaxNode
    {
        private SyntaxNode _leftChild;
        private SyntaxNode _rightChild;

        public SyntaxNode Parent { get; private set; }

        public SyntaxNode Root => Parent?.Root ?? this;

        public virtual SyntaxNode LeftChild 
        { 
            get => _leftChild;
            set
            {
                _leftChild = value;
                if (value != null) _leftChild.Parent = this;
            }
        }

        public virtual SyntaxNode RightChild 
        { 
            get => _rightChild;
            set
            {
                _rightChild = value;
                if (value != null) _rightChild.Parent = this;
            }
        }

        public bool HasPriority { get; internal set; }

        public abstract void Accept(IVisitor visitor);
    }
}
