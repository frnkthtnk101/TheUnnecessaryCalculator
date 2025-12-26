using System.Text;
using System.Text.RegularExpressions;

namespace mycalculator
{
    /// <summary>
    /// Defines methods for performing calculations, retrieving the associated regular expression,  and accessing
    /// calculation logs.
    /// </summary>
    /// <remarks>This interface provides a contract for managing calculations, including processing input
    /// data,  retrieving the regular expression used for validation or parsing, and accessing logs of previous
    /// calculations. Implementations of this interface should define the specific behavior for these
    /// operations.</remarks>
    public interface ICalculatorManager
    {
        public string DoCalculation(string input);
        public string GetRegex();
        public string GetLog();
    }
    /// <summary>
    /// Provides functionality to perform mathematical calculations based on a string input.
    /// </summary>
    /// <remarks>The <see cref="CalculatorManager"/> class processes mathematical expressions provided as
    /// strings and performs calculations using a custom calculator mechanism. Supported operations include addition,
    /// multiplication, and exponentiation. The input string must follow a specific format, where numbers and operators
    /// are separated by spaces (e.g., "2 + 3 * 4"). <para> This class also supports verbose logging of intermediate
    /// calculation steps, which can be enabled through the constructor. </para></remarks>
    public class CalculatorManager : ICalculatorManager
    {
        bool _verbose;
        readonly string _regexForInput;
        StringBuilder _log;
        Operation _operation;
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorManager"/> class.
        /// </summary>
        /// <param name="verbose">A value indicating whether the calculator should operate in verbose mode.  If <see langword="true"/>,
        /// additional details about operations may be logged or displayed.</param>
        public CalculatorManager(bool verbose)
        {
            _verbose = verbose;
            _regexForInput = @"(\d+)|(\+|\*|\^|\-|\/)";
        }
        /// <summary>
        /// Parses the input string, performs mathematical operations based on the detected operators,  and returns the
        /// result as a string.
        /// </summary>
        /// <remarks>The method processes the input string sequentially, applying the detected operations
        /// (+, *, ^)  to the numbers in the order they appear. The operations are performed using an internal
        /// calculator object.</remarks>
        /// <param name="input">A string containing numbers and mathematical operators (+, *, ^) to be processed.  The input must contain at
        /// least three valid matches (numbers or operators).</param>
        /// <returns>A string representation of the calculated result.</returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="input"/> contains fewer than three valid matches,  or if the input contains
        /// invalid characters that are neither numbers nor supported operators.</exception>
        public string DoCalculation(string input)
        {
           Regex matches = new Regex(_regexForInput);
            var matchCollection = matches.Matches(input);
            if(matchCollection.Count < 3) 
                throw new ArgumentException($"Invalid Input given {input}");
            Calculator calculator = new Calculator();
            _log = new StringBuilder();
            foreach (var match in matchCollection) 
            { 
                if(match.ToString() == "+")
                {
                    calculator.SetOperation(Operation.Add);
                    _operation = Operation.Add;
                }
                else if (match.ToString() == "*")
                {
                    calculator.SetOperation(Operation.Add);
                    _operation = Operation.Mulitply;
                }
                else if(match.ToString() == "^")
                {
                    calculator.SetOperation(Operation.Add);
                    _operation = Operation.POW;
                }
                else if(match.ToString() == "-")
                {
                    calculator.SetOperation(Operation.Subtract);
                    _operation = Operation.Subtract;
                }
                else if(match.ToString() == "/")
                {
                    calculator.SetOperation(Operation.Subtract);
                    _operation = Operation.Divide;
                }
                else if (int.TryParse(match.ToString(), out int number))
                {

                    DoMath(calculator, number);
                    _operation = Operation.NoOp;
                }
                else
                {
                    throw new ArgumentException($"Invalid number input: {match}");
                }

            }
            _operation = Operation.NoOp;
            var resultString = calculator.ShowResult();
            if (string.IsNullOrEmpty(resultString))
            {
                return "0";
            }
            return calculator.ShowResult();
        }
        /// <summary>
        /// Performs a mathematical operation on the specified <see cref="Calculator"/> instance using the provided
        /// number.
        /// </summary>
        /// <remarks>The operation performed depends on the current state of the internal operation mode:
        /// multiplication, power, or addition. If the calculator's result is invalid or the calculator is in an
        /// inconsistent state, an exception is thrown.</remarks>
        /// <param name="calculator">The <see cref="Calculator"/> instance on which the operation will be performed. Must not be <c>null</c>.</param>
        /// <param name="number">The number to be used in the operation. Must be a positive integer.</param>
        /// <exception cref="Exception">Thrown if the calculator's result is invalid or the calculator is in an inconsistent state.</exception>
        private void DoMath( Calculator calculator, int number)
        {
            var resultString = calculator.ShowResult();
            if (string.IsNullOrEmpty(resultString) == false &
                !int.TryParse(resultString, out int currentResult))
            {
                throw new Exception("Calculator is broken.");
            }
            if (_operation == Operation.Mulitply)
            {
                DoMulitiplication(calculator, number);
            }
            else if (_operation == Operation.Divide)
            {
                if (number == 0)
                {
                    throw new DivideByZeroException("Cannot divide by zero.");
                }
                int quotient = 0;
                var divisor = number;
                var dividend = currentResult;
                while (dividend >= divisor)
                {
                    dividend -= divisor;
                    calculator.SetInput(divisor);
                    calculator.Rotatehandle();
                    quotient++;
                }
                //you're going to have to reset it.
                calculator.SetInput(quotient);
                calculator.Rotatehandle();
            }
            else if (_operation == Operation.POW)
            {
                for (int i = 1; i < number; i++)
                    DoMulitiplication(calculator, currentResult);
            }
            else // addition & subtraction
            { 
                calculator.SetInput(number);
                calculator.Rotatehandle();
            }
            if (_verbose)
            {
                _log.AppendLine($"After input {number}, intermediate result is: {calculator.ShowResult()}");
            }
        }
        /// <summary>
        /// Performs a multiplication operation using the specified calculator and multiplier.
        /// </summary>
        /// <remarks>This method uses the calculator's current result as the base value and multiplies it
        /// by the specified multiplier. If either the current result or the multiplier is zero, the calculator's input
        /// is set to zero, and no further operations are performed.</remarks>
        /// <param name="calculator">The <see cref="Calculator"/> instance used to perform the operation. Must not be <c>null</c>.</param>
        /// <param name="number">The multiplier for the operation. Must be a non-negative integer.</param>
        /// <exception cref="InvalidOperationException">Thrown if the calculator's current result is not a valid integer.</exception>
        private void DoMulitiplication(Calculator calculator, int number)
        {
            var resultString = calculator.ShowResult();
            int resultNumber = 0;
            if (string.IsNullOrEmpty(resultString) ||
                !int.TryParse(resultString, out resultNumber))
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
        /// <summary>
        /// Retrieves the regular expression used for input validation.
        /// </summary>
        /// <returns>The regular expression as a string.</returns>
        public string GetRegex() => _regexForInput;
        /// <summary>
        /// Retrieves the current log as a string.
        /// </summary>
        /// <returns>A string representation of the current log. Returns an empty string if the log is empty.</returns>
        public string GetLog() => _log.ToString();

    }
}
