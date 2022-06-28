using Calc.SyntaxNodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class LogInterpreter : Interpreter, IDisposable
    {
        private readonly StreamWriter _logWriter;

        public LogInterpreter()
        {
            _logWriter = new StreamWriter("log.txt");
        }

        public void Dispose()
        {
            _logWriter.Dispose();
        }

        public override void Visit(ArithmeticOperation operation)
        {
            base.Visit(operation);
            _logWriter.WriteLine(operation.ToString());
        }

        public override void Visit(Constant constant)
        {
            base.Visit(constant);
            _logWriter.WriteLine(constant.ToString());
        }
    }
}
