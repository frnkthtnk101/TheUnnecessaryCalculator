using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycalculator
{
    //For implementing different calculator interfaces. 
    //Used to separate the core calculator logic from the user interface..
    public interface CalculatorInterface
    {
        /// <summary>
        /// Runs the calculator interface. It
        /// could be terminal-based, GUI-based.
        /// </summary>
        void Run();
    }
    /// <summary>
    /// A terminal-based implementation of the CalculatorInterface.
    /// </summary>
    public class TerminalCalculator : CalculatorInterface
    {
        /// <summary>
        /// Runs the terminal calculator interface.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
