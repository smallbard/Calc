using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Tests
{
    [TestClass]
    public class InterpreterTests
    {
        [TestMethod]
        public void Number()
        {
            var interpreter = new Interpreter();
            var ast = new Parser().Parse(new Tokenizer().Tokenize("33"));
            ast.Accept(interpreter);

            Assert.AreEqual(33, interpreter.Result);
        }

        [TestMethod]
        public void SimpleOperation()
        {
            var interpreter = new Interpreter();
            var ast = new Parser().Parse(new Tokenizer().Tokenize("100+3*5"));
            ast.Accept(interpreter);

            Assert.AreEqual(115, interpreter.Result);
        }

        [TestMethod()]
        [DataRow("((100+300)*200)*400", 32000000)]
        [DataRow("400*((100+300)*200)", 32000000)]
        public void NestedBrackets(string expression, int result)
        {
            var interpreter = new Interpreter();
            var ast = new Parser().Parse(new Tokenizer().Tokenize(expression));
            ast.Accept(interpreter);

            Assert.AreEqual(result, interpreter.Result);
        }
    }
}
