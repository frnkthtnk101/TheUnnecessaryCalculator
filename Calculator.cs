using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycalculator
{
    public class Calculator
    {
        readonly int _numberOfModules;// number of digits the calculator can handle
        Module[] _modules;// array of modules representing each digit position
        Operation _operation;// current operation (add or subtract)
        bool _finalCarryOver;// indicates if there was a carry over in the final module
        public Calculator()
        {
            _numberOfModules = 10;
            _modules = new Module[_numberOfModules];// initialize with 10 drums for a 10-digit calculator
            _operation = Operation.Add;// default operation is addition
            for (int i = 0; i < _modules.Length; i++)
            {
                _modules[i] = new Module();
                _modules[i].SetPostion(0);
                _modules[i].SetOperation(_operation);
            }
        }
        public void SetOperation(Operation operation)
        {
            _operation = operation;
            foreach (var module in _modules)
            {
                module.SetOperation(operation);
            }
        }
        public void SetInput(int number)
        {
            if (_operation == Operation.NoOp)
            {
                return;
            }
            var places = (int)Math.Pow(10, (_numberOfModules - 1));
            bool isNegative = number < 0;

            if (_operation == Operation.Subtract && isNegative)
            {
                number = Math.Abs(number); // Convert to positive for processing
            }

            for (int i = _numberOfModules - 1; i >= 0; i--)
            {
                var digit = number / places;
                _modules[i].SetPostion(digit);
                number = number % places;
                places /= 10;
            }

            if (_operation == Operation.Subtract && isNegative)
            {
                // If the number was negative, set the most significant module to indicate negativity
                _modules[_numberOfModules - 1].SetPostion(-_modules[_numberOfModules - 1].GetResultDigit());
            }
        }
        public void Rotatehandle()
        {
            int i = -1;
            for (i = 0; i < _numberOfModules; i++)
            {
                DoRotation(i);
            }

        }

        private void DoRotation(int i)
        {

            _modules[i].Rotate();

            // capture carry/borrow from the module that just rotated
            bool hasCarry = _modules[i].CarryOver;

            if (!hasCarry) return;

            int nextSpot = i + 1; // don't mutate i!

            if (nextSpot < _numberOfModules)
            {
                // If you support subtraction, swap DoCarry for DoBorrow when needed
                if (_operation == Operation.Add)
                    _modules[nextSpot].DoCarry();
                else
                    _modules[nextSpot].DoBorrow(); // ensure Module implements this

                _modules[i].CleanCarryOver(); // clear on the source of the carry/borrow
            }
            else
            {
                // overflow (for add) or underflow (for subtract) at the most significant digit
                _finalCarryOver = true;
                _modules[i].CleanCarryOver();
            }
        }

        
        public int ShowResult()
        {
            StringBuilder result = new StringBuilder();
            var digitAppeared = false;
            if (_finalCarryOver)
            {
                result.Append("1");
                digitAppeared = true;
            }
            foreach(var module in _modules.Reverse())
            {
                var digit = module.GetResultDigit();
                if(!digitAppeared && digit == 0)
                {
                    continue;
                }
                else
                {
                    digitAppeared = true;
                    result.Append(module.GetResultDigit().ToString());
                }

            }
            int.TryParse(result.ToString(), out int finalResult);
            return finalResult;
        }
    }
}
