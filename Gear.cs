using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycalculator
{
    public enum Operation
    {
        Add,
        Subtract, // not ready yet
        Mulitply,
    }
    public class Module
    {
        SteppedDrum drum;// this is the main drum that holds all the gears
        Digit _position;// this is the  pointer that shows which gear is active
        ResultsWheel _resultsWheel;// this is the results wheel that shows the result of the operation
        Operation _operation;// this is the operation to be performed (add or subtract)
        public Module()
        {
            drum = new SteppedDrum();
            _position = 0;
            _resultsWheel = new ResultsWheel();
            _operation = Operation.Add;
        }
        public void SetPostion(Digit position)
        {
            _position = position;
        }
        public void Rotate()
        {
            var numberOfRotations = drum.Rotate(_position);
            for(int i = 0; i < numberOfRotations; i++)
            {
                if(_operation == Operation.Subtract)
                    _resultsWheel.Subtract();
                else
                    _resultsWheel.Add();
            }
        }
        public void SetOperation(Operation operation)
        {
            _operation = operation;
        }

        internal void DoCarry()
        {
           if(_operation == Operation.Add)
                _resultsWheel.Add();
        }
        internal void DoBorrow()
        {
            if (_operation == Operation.Subtract)
                _resultsWheel.Subtract();
        }
        internal Digit GetResultDigit()
        {
            return _resultsWheel.Result;
        }

        internal void CleanCarryOver()
        {
            _resultsWheel.CleanCarryOver();
        }

        public bool CarryOver
        {
            get { return _resultsWheel.CarryOver; }
        }

    }
    /// <summary>
    /// Represents a single digit wheel in a results mechanism, capable of incrementing and decrementing its value.
    /// </summary>
    /// <remarks>The <see cref="ResultsWheel"/> maintains a single digit value between 0 and 9.  When the
    /// value reaches its upper or lower limit during an operation, it wraps around  and sets a carry-over flag to
    /// indicate the overflow or underflow condition.</remarks>
    public class ResultsWheel
    {
        Digit _value;
        bool _carryOver;
        public ResultsWheel()
        {
            _value = 0;
        }

        public bool CarryOver => _carryOver;
        public void CleanCarryOver()
        {
            _carryOver = false;
        }
        public Digit Result => _value;

        public void Add()
        {
            if (_value == 9)
            {
                _value = 0;
                _carryOver = true;
            }
            else
            {
                _value += 1;
            }

        }
        public void Subtract()
        {
            if (_value == 0)
            {
                _value = 9;
                _carryOver = true;
            }
            else
            {
                _value -= 1;
            }

        }
    }
    /// <summary>
    /// Represents a stepped drum mechanism composed of multiple gears, each with a specific number of teeth.
    /// </summary>
    /// <remarks>The <see cref="SteppedDrum"/> class models a mechanical stepped drum, where each gear
    /// corresponds to a digit  and has a predefined number of teeth. The drum can be rotated to interact with a
    /// specific gear, and the rotation  operation calculates a value based on the number of teeth in the selected
    /// gear.</remarks>
    public class SteppedDrum
    {
        Gear[] gears;
        public SteppedDrum()
        {
            gears = new Gear[10];
            for (int i = 0; i < 10; i++)
            {
                gears[i] = new Gear(i);
            }
        }
        /// <summary>
        /// Rotates the gear at the specified position and calculates the total number of rotations.
        /// </summary>
        /// <param name="position">The position of the gear to rotate. Must correspond to a valid gear in the collection.</param>
        /// <returns>The total number of rotations performed, equal to the number of teeth on the specified gear.</returns>
        public Digit Rotate(Digit position)
        {
            Digit result = 0;
            var numberOfTeeth = gears[position].Teeth;
            for (Digit i = 0; i < numberOfTeeth; i++)
            {
                result += 1;
            }
            return result;
        }
    }
    /// <summary>
    /// Represents a gear with a specific number of teeth.
    /// </summary>
    /// <remarks>The <see cref="Gear"/> class encapsulates the concept of a gear, which is defined by its
    /// number of teeth. This class is immutable after initialization, as the number of teeth cannot be changed once the
    /// gear is created.</remarks>
    public class Gear
    {
        Digit _teeth;
        public Gear(Digit teeth)
        {
            _teeth = teeth;
        }
        public Digit Teeth
        {
            get { return _teeth; }
        }
    }

    public struct Digit
    {
        private int _value;
        public Digit(int value)
        {
            if (value < 0 || value > 9)
                throw new ArgumentOutOfRangeException("Digit must be between 0 and 9.");
            _value = value;
        }
        public int Value => _value;

        // Optional: convenience conversions (keeps your API terse)
        public static implicit operator Digit(int value) => new Digit(value);
        public static implicit operator int(Digit d) => d._value;
        public static bool operator <(Digit d1, Digit d2) => d1._value < d2._value;
        public static bool operator >(Digit d1, Digit d2) => d1._value > d2._value;
        public static bool operator <=(Digit d1, Digit d2) => d1._value <= d2._value;
        public static bool operator >=(Digit d1, Digit d2) => d1._value >= d2._value;
        public override string ToString() => _value.ToString();
    }
}
