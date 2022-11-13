using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;


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

            UseExpression();

            #region Generic type
            List<int> listInt = new List<int>();
            List<string> listStr = new List<string>();

            listInt.Add(3);

            listStr.Add("Vlad");
            listStr.Add("Andrenkov");

            List<MyClass> objs = new List<MyClass>
            {
                new MyClass()
                {
                    Name= "Vladimir",
                }
            };

            UseGenericType(listInt, objs, listStr) ;
            #endregion

            //Invoke
            InpokeAndPrint();
            //Or
            Console.WriteLine("Older than 70:");
            GetMyPerson();

            Console.Read();
        }


        /// <summary>
        /// Lambda, Func and Action
        /// </summary>
        static void UseLambda()
        {
            #region Func<>
            //Func<parameter1, parameter2, returntype>
            // (x, y) "Goes to" { return 3 * x; };
            Func<double, int, double> myF = (x, y) => { return 3 * x; };
            Console.WriteLine($"Func() x = {myF(3.62, 6)}");
            #endregion

            #region Action<>
            //Action<param1, param2>
            //It denotes the Generic delegate here that have return type void
            Action<double, int> myA = (x, y) => { 
                Console.WriteLine($"Action() x = {x * y}"); 
            };
            myA(5.25, 3);
            #endregion
        }

        /// <summary>
        /// Expression
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void UseExpression()
        {
            //https://www.c-sharpcorner.com/UploadFile/vendettamit/beginners-guide-for-expression-trees-in-C-Sharp-understanding-e/

            // A simple delegated operation which perform string join.  
            Func<string, string, string> StringJoinOld = (str1, str2) => string.Concat(str1, str2);

            //Old style
            Console.WriteLine("Old style: " + StringJoinOld("ABCD", "efhg"));

            //New - treat this code as Data
            //Expression structure:
            // 1. Body        - Concat(str1, str2)
            // 2. Parameters  - str1, str2
            // 3. Node Type   - Lambda
            // 4. Return Type - String

            Expression<Func<string, string, string>> StringJoinExpr = (str1, str2) => string.Concat(str1, str2);

            //Unlike Func<> and Action<> expression are not compile time unit
            //So to execute the our Expression we must first compile it.

            //var func = StringJoinExpr.Compile();
            //Console.WriteLine("New style: " + func("Smith", "Jones"));

            //or

            var result = StringJoinExpr.Compile()("Smith", "Jones");
            Console.WriteLine("New style: " + result);

            //Invoice use LinQ param types
            List<MyClass> myClass = new List<MyClass>();
            myClass.Add(new MyClass() { Name = "Name A" });
            myClass.Add(new MyClass() { Name = "Name B" });

            var tempClass = myClass.Find(n => n.Name == "Name A").Name;

            //var resultX = GetMyClassName.Invoke(tempClass);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// TypeParam supposed to be an Interface!!!!
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TInt"></typeparam>
        /// 
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private static void UseGenericType<T, TEnum, TInt>(TInt arg1, T arg2, TEnum arg3)
            where TInt : IList, new()
            where TEnum : IEnumerable<string>, new()
            where T : IEnumerable<MyClass>, new()
        {
            var res = arg1.Cast<int>()
                .SingleOrDefault();

            var myClass = arg2.FirstOrDefault(n => n.Name != "");

            PrintNextFromEnum("\n" + myClass.Name + ":\n");
            for (int i = 0; i < res; i++)
            {
                PrintNextFromEnum(arg3.FirstOrDefault() +" "+ arg3.LastOrDefault());
            }

            foreach (string item in BuildStrFromEnum())
            {
                PrintNextFromEnum(item);
            }

            #region Local function
            void PrintNextFromEnum(string str)
            {
                Console.WriteLine(str);
            }

            ///
            ///Returns collection of values 
            ///
            IEnumerable<string> BuildStrFromEnum()
            {
                yield return "Do 1";
                yield return "Do 2";
            }
            #endregion

        }


        public static Expression<Func<MyClass, string>> GetMyClassName(MyClass m)
        {
            
            return x => x.Name;
        }

        #region Invoke example
        //https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.invoke?view=netframework-4.7.2
        private static void InpokeAndPrint()
        {
            Console.WriteLine(invocationExpression.ToString());
        }

        public static Expression<Func<string, string, string>> GetFullName = (part1, part2) => 
         (part1 + part2);

        // Create an InvocationExpression that represents applying
        // the arguments 'Vlad' and 'Andrenkov' to the lambda expression 'GetFullName'.
        public static InvocationExpression invocationExpression = Expression.Invoke(GetFullName,
                                                                                    Expression.Constant("Vlad"),
                                                                                    Expression.Constant("Andrenkov"));

        #region Another Invoice example
        //https://csharp.hotexamples.com/examples/System.Linq.Expressions/Expression/Invoke/php-expression-invoke-method-examples.html

        public static IEnumerable<MyClass> GetResults(Expression<Func<MyClass, bool>> criteria)
        {
            IQueryable<MyClass> personeList = new[] {
                                            new MyClass { Name = "Vlad", Age = 58},
                                            new MyClass { Name = "Irina", Age = 71 },
                                            new MyClass { Name = "Augusta", Age = 93 }
                                        }.AsQueryable();

            var query = from person in personeList.AsExpandable()//needs LinqKit
                        where criteria.Invoke(person)//invokes Expression "b => b.Age > 70)"
                        select person;

            return query;
        }

        public static void GetMyPerson()
        {
            foreach (var person in GetResults(b => b.Age > 70))
            {
                Console.WriteLine(person.Name);
            }
        }

        #endregion

        #endregion
    }
}
