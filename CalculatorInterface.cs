using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        Calculator _calculator;
        readonly int _bottomRow;
        readonly Regex _mathCalculation;
        
        public TerminalCalculator()
        {
            _calculator = new Calculator();
            string regexForInput = @"(\d+)|(\+|\*)";
            _mathCalculation = new Regex(regexForInput);

        }
        /// <summary>
        /// Runs the terminal calculator interface.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Run()
        {
            
            Console.CursorVisible = false;
            while (true) 
            {
                Console.WriteLine("Give me a calculation - addition and mulitplication only.");
                var input = Console.ReadLine();
                if (_mathCalculation.IsMatch(input)) DoCalculation(input);
                if (input.ToUpper() == "Q")
                    break;

            }

        }

        void DoCalculation(string input)
        {
            _calculator = new Calculator();
            var matches = _mathCalculation.Matches(input);
            if (matches.Count < 3) return;
            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (int.TryParse(match.Value, out int number))
                {
                    _calculator.SetInput(number);
                }
                else
                {
                    if (match.Value == "+")
                        _calculator.SetOperation(Operation.Add);
                    else if (match.Value == "*")
                        _calculator.SetOperation(Operation.Mulitply);
                    _calculator.Rotatehandle();
                }
            }
            _calculator.Rotatehandle();
            Console.WriteLine("Result: " + _calculator.ShowResult());
        }
    }
}
