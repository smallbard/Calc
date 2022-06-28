using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class InterpreterFactory
    {
        public Interpreter CreateInterpreter()
        {
            if (bool.TryParse(ConfigurationManager.AppSettings["Log"], out var log) && log)
            {
                return new LogInterpreter();
            }

            return new Interpreter();
        }
    }
}
