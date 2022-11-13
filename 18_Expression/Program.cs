using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

//https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/
//https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions

namespace ExpressionTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Expression");

            //TestExpression();
            // Creating an expression tree.
            Expression<Func<int, bool>> expr = num => num < 5;

            // Compiling the expression tree into a delegate.  
            Func<int, bool> result = expr.Compile();

            // Invoking the delegate and writing the result to the console.  
            Console.WriteLine(result(4));

            UseLambda();

            Console.Read();
        }

        static void UseLambda()
        {
            //Func<>
            //Func<parameter1, parameter2, returntype>
            // (x, y) "Goes to" { return 3 * x; };
            Func<double, int, double> myF = (x, y) => { return 3 * x; };
            Console.WriteLine($"Func() x = {myF(3.62, 6)}");

            //Action<>
            //Action<param1, param2>
            //It denotes the Generic delegate here that have return type void
            Action<double, int> myA = (x, y) => { Console.WriteLine($"Action() x = {x * y}"); };
            myA(5.25, 3);
            //Console.WriteLine($"Action() x = {myA(5.25, 6)}");
        }
    }
}
