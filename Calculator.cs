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
            
            if (_operation == Operation.Add || _operation == Operation.Mulitply)
            {
                var places = (int)Math.Pow(10, (_numberOfModules - 1));
                for (int i = _numberOfModules - 1; i >= 0; i--)
                {
                    var digit = number / places;
                    _modules[i].SetPostion(digit);
                    number = number % places;
                    places /= 10;
                }
            }
            else // subtraction
            {
                var places = 1;
                for (int i = _numberOfModules - 1; i >= 0; i--)
                {
                    var digit = number / places;
                    _modules[i].SetPostion(digit);
                    number = number % places;
                    places *= 10;
                }
            }


        }
        public void Rotatehandle(int numberOfTimes = 0)
        {
            int i = -1;
            //if (_operation == Operation.Mulitply && numberOfTimes != 0)
            //{
            //    Rotatehandle(numberOfTimes - 1);
            //}
            //else if (_operation == Operation.Mulitply)
            //{
            //    return;
            //}

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


        public string ShowResult()
        {
            StringBuilder result = new StringBuilder();
            var digitAppeared = false;
            if(_finalCarryOver)
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
            return result.ToString();
        }
    }
}
