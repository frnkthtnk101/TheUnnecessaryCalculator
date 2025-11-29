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

    public abstract class BaseGUI : CalculatorInterface
    {
        protected CalculatorManager _calculator;
        protected bool _verbose;
        protected BaseGUI(bool verbose)
        {
            _verbose = verbose;
            _calculator = new CalculatorManager(_verbose);
        }
        public void Run()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A terminal-based implementation of the CalculatorInterface.
    /// </summary>
    public class TerminalCalculator : BaseGUI
    {
        
        readonly int _bottomRow;
        readonly Regex _mathCalculation;

        public TerminalCalculator(bool _verbose) : base(_verbose)
        {
            base._calculator = new CalculatorManager(_verbose);
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
            var correctInput = new Regex(_calculator.GetRegex());
            if (!correctInput.IsMatch(input))
            {
                Console.WriteLine("Invalid input. Please provide a valid calculation.");
                return;
            }
            var result = _calculator.DoCalculation(input);
            Console.WriteLine($"Result: {result}");
            if(_verbose)
            {
                Console.WriteLine("Log:");
                Console.WriteLine(_calculator.GetLog());
            }
        }
    }
}
