using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mycalculator
{
    public interface ICalculatorManager
    {
        public string DoCalculation(string input);
        public string GetRegex();
        public string GetLog();
    }
    public class CalculatorManager : ICalculatorManager
    {
        bool _verbose;
        readonly string _regexForInput;
        StringBuilder _log;
        public CalculatorManager(bool verbose)
        {
            _verbose = verbose;
            _regexForInput = @"(\d+)|(\+|\*)";
        }
        public string DoCalculation(string input)
        {
           Regex matches = new Regex(_regexForInput);
            var matchCollection = matches.Matches(input);
            if(matchCollection.Count < 3) 
                throw new ArgumentException($"Invalid Input given {input}");
            var lastOperationWasMultiply = false;
            Calculator calculator = new Calculator();
            _log = new StringBuilder();
            foreach (var match in matchCollection) 
            { 
                if(match.ToString() == "+")
                {
                    calculator.SetOperation(Operation.Add);
                    lastOperationWasMultiply = false;
                }
                else if (match.ToString() == "*")
                {
                    calculator.SetOperation(Operation.Add);
                    lastOperationWasMultiply = true;
                }
                else if (int.TryParse(match.ToString(), out int number))
                {   
                    if(lastOperationWasMultiply)
                    {
                        DoMulitiplication(calculator, number);
                        lastOperationWasMultiply = false;
                    }
                    else // addition
                    {
                        calculator.SetInput(number);
                        calculator.Rotatehandle();
                    }
                    if (_verbose)
                    {
                         _log.AppendLine($"After input {number}, intermediate result is: {calculator.ShowResult()}");
                    }
                }
                else
                {
                    throw new ArgumentException($"Invalid number input: {match}");
                }

            }
            return calculator.ShowResult();
        }

        private void DoMulitiplication(Calculator calculator, int number)
        {
            var resultString = calculator.ShowResult();
            if (!int.TryParse(resultString, out int resultNumber))
            {
                throw new InvalidOperationException("Calculator result is not a valid integer: " + resultString);
            }
            if (resultNumber == 0 || number == 0)
            {
                // Multiplication by zero results in zero
                calculator.SetInput(0);
                calculator.Rotatehandle();
            }
            calculator.SetInput(resultNumber);
            // Rotate the handle (number - 1) times for multiplication
            for (int i = 1; i < number; i++)
                calculator.Rotatehandle(); 
        }

        public string GetRegex() => _regexForInput;

        public string GetLog() => _log.ToString();

    }
}
