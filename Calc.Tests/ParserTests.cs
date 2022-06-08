using Calc.SyntaxNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void JustANumber()
        {
            var ast = new Parser().Parse(new Tokenizer().Tokenize("100"));
            Assert.IsNotNull(ast);

            Assert.IsInstanceOfType(ast, typeof(Constant));
            Assert.AreEqual(100, ((Constant)ast).Value);
        }

        [TestMethod]
        public void SimpleExpression()
        {
            var ast = new Parser().Parse(new Tokenizer().Tokenize("100+300-400"));
            Assert.IsNotNull(ast);

            Assert.IsInstanceOfType(ast, typeof(ArithmeticOperation));
            var addition = (ArithmeticOperation)ast;
            Assert.AreEqual(Operator.Add, addition.Operator);

            Assert.IsInstanceOfType(addition.LeftChild, typeof(Constant));
            Assert.AreEqual(100, ((Constant)addition.LeftChild).Value);

            Assert.IsInstanceOfType(addition.RightChild, typeof(ArithmeticOperation));
            var substraction = (ArithmeticOperation)addition.RightChild;
            Assert.AreEqual(Operator.Substract, substraction.Operator);

            Assert.IsInstanceOfType(substraction.LeftChild, typeof(Constant));
            Assert.AreEqual(300, ((Constant)substraction.LeftChild).Value);

            Assert.IsInstanceOfType(substraction.RightChild, typeof(Constant));
            Assert.AreEqual(400, ((Constant)substraction.RightChild).Value);

        }

        [TestMethod]
        public void ExpressionWithPriority()
        {
            var ast = new Parser().Parse(new Tokenizer().Tokenize("100*300+400"));
            Assert.IsNotNull(ast);

            Assert.IsInstanceOfType(ast, typeof(ArithmeticOperation));
            var addition = (ArithmeticOperation)ast;
            Assert.AreEqual(Operator.Add, addition.Operator);

            Assert.IsInstanceOfType(addition.RightChild, typeof(Constant));
            Assert.AreEqual(400, ((Constant)addition.RightChild).Value);

            Assert.IsInstanceOfType(addition.LeftChild, typeof(ArithmeticOperation));
            var multiplication = (ArithmeticOperation)addition.LeftChild;
            Assert.AreEqual(Operator.Multiply, multiplication.Operator);

            Assert.IsInstanceOfType(multiplication.LeftChild, typeof(Constant));
            Assert.AreEqual(100, ((Constant)multiplication.LeftChild).Value);

            Assert.IsInstanceOfType(multiplication.RightChild, typeof(Constant));
            Assert.AreEqual(300, ((Constant)multiplication.RightChild).Value);
        }

        [TestMethod]
        public void ExpressionWithBrakets()
        {
            var ast = new Parser().Parse(new Tokenizer().Tokenize("(100+300)*400"));
            Assert.IsNotNull(ast);

            Assert.IsInstanceOfType(ast, typeof(ArithmeticOperation));
            var multiply = (ArithmeticOperation)ast;
            Assert.AreEqual(Operator.Multiply, multiply.Operator);

            Assert.IsInstanceOfType(multiply.RightChild, typeof(Constant));
            Assert.AreEqual(400, ((Constant)multiply.RightChild).Value);

            Assert.IsInstanceOfType(multiply.LeftChild, typeof(ArithmeticOperation));
            var addition = (ArithmeticOperation)multiply.LeftChild;
            Assert.AreEqual(Operator.Add, addition.Operator);

            Assert.IsInstanceOfType(addition.LeftChild, typeof(Constant));
            Assert.AreEqual(100, ((Constant)addition.LeftChild).Value);

            Assert.IsInstanceOfType(addition.RightChild, typeof(Constant));
            Assert.AreEqual(300, ((Constant)addition.RightChild).Value);

            ast = new Parser().Parse(new Tokenizer().Tokenize("400*(100+300)"));
            Assert.IsNotNull(ast);

            Assert.IsInstanceOfType(ast, typeof(ArithmeticOperation));
            multiply = (ArithmeticOperation)ast;
            Assert.AreEqual(Operator.Multiply, multiply.Operator);

            Assert.IsInstanceOfType(multiply.LeftChild, typeof(Constant));
            Assert.AreEqual(400, ((Constant)multiply.LeftChild).Value);

            Assert.IsInstanceOfType(multiply.RightChild, typeof(ArithmeticOperation));
            addition = (ArithmeticOperation)multiply.RightChild;
            Assert.AreEqual(Operator.Add, addition.Operator);

            Assert.IsInstanceOfType(addition.LeftChild, typeof(Constant));
            Assert.AreEqual(100, ((Constant)addition.LeftChild).Value);

            Assert.IsInstanceOfType(addition.RightChild, typeof(Constant));
            Assert.AreEqual(300, ((Constant)addition.RightChild).Value);
        }

        [TestMethod]
        public void ExpressionWithNestedBrackets()
        {
            var ast = new Parser().Parse(new Tokenizer().Tokenize("((100+300)*200)*400"));
            Assert.IsNotNull(ast);

            Assert.IsInstanceOfType(ast, typeof(ArithmeticOperation));
            var multiply = (ArithmeticOperation)ast;
            Assert.AreEqual(Operator.Multiply, multiply.Operator);

            Assert.IsInstanceOfType(multiply.RightChild, typeof(Constant));
            Assert.AreEqual(400, ((Constant)multiply.RightChild).Value);

            Assert.IsInstanceOfType(multiply.LeftChild, typeof(ArithmeticOperation));
            var multiply2 = (ArithmeticOperation)multiply.LeftChild;
            Assert.AreEqual(Operator.Multiply, multiply2.Operator);

            Assert.IsInstanceOfType(multiply2.RightChild, typeof(Constant));
            Assert.AreEqual(200, ((Constant)multiply2.RightChild).Value);

            Assert.IsInstanceOfType(multiply2.LeftChild, typeof(ArithmeticOperation));
            var addition = (ArithmeticOperation)multiply2.LeftChild;
            Assert.AreEqual(Operator.Add, addition.Operator);

            Assert.IsInstanceOfType(addition.LeftChild, typeof(Constant));
            Assert.AreEqual(100, ((Constant)addition.LeftChild).Value);

            Assert.IsInstanceOfType(addition.RightChild, typeof(Constant));
            Assert.AreEqual(300, ((Constant)addition.RightChild).Value);

            ast = new Parser().Parse(new Tokenizer().Tokenize("400*((100+300)*200)"));
            Assert.IsNotNull(ast);

            Assert.IsInstanceOfType(ast, typeof(ArithmeticOperation));
            multiply = (ArithmeticOperation)ast;
            Assert.AreEqual(Operator.Multiply, multiply.Operator);

            Assert.IsInstanceOfType(multiply.LeftChild, typeof(Constant));
            Assert.AreEqual(400, ((Constant)multiply.LeftChild).Value);

            Assert.IsInstanceOfType(multiply.RightChild, typeof(ArithmeticOperation));
            multiply2 = (ArithmeticOperation)multiply.RightChild;
            Assert.AreEqual(Operator.Multiply, multiply2.Operator);

            Assert.IsInstanceOfType(multiply2.RightChild, typeof(Constant));
            Assert.AreEqual(200, ((Constant)multiply2.RightChild).Value);

            Assert.IsInstanceOfType(multiply2.LeftChild, typeof(ArithmeticOperation));
            addition = (ArithmeticOperation)multiply2.LeftChild;
            Assert.AreEqual(Operator.Add, addition.Operator);

            Assert.IsInstanceOfType(addition.LeftChild, typeof(Constant));
            Assert.AreEqual(100, ((Constant)addition.LeftChild).Value);

            Assert.IsInstanceOfType(addition.RightChild, typeof(Constant));
            Assert.AreEqual(300, ((Constant)addition.RightChild).Value);
        }
    }
}
