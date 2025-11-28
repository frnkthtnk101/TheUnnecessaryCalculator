
using mycalculator;

while (true)
{
    Calculator calc = new Calculator();
    Console.WriteLine("Enter equation like 23 + 45 or 67 - 12 (or type 'exit' to quit):");
    var input = Console.ReadLine();
    if (input.ToLower() == "exit")
        break;
    var parts = input.Split(' ');
    if (parts.Length != 3 || !int.TryParse(parts[0], out int num1) || !int.TryParse(parts[2], out int num2) || (parts[1] != "+" && parts[1] != "-"))
    {
        Console.WriteLine("Invalid input. Please try again.");
        continue;
    }
    calc.SetInput(num1);
    calc.SetOperation(parts[1] == "+" ? Operation.Add : Operation.Subtract);
    calc.Rotatehandle();
    calc.SetInput(num2);
    calc.Rotatehandle();
    Console.WriteLine("Result: " + calc.ShowResult());
}


