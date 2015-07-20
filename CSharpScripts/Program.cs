using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.CSharp;
using System;

namespace CSharpScripts
{
    public class Program
    {
        public class GlobalParams
        {
            public int X;
            public int Y;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("EvalWithoutParameters");
            EvalWithoutParameters();

            Console.WriteLine("EvalExpression");
            EvalExpression();

            Console.WriteLine("ScriptCreateAndRun");
            ScriptCreateAndRun();

            Console.WriteLine("ScriptWithNamespace");
            ScriptWithNamespace();

            Console.WriteLine("ScriptAsDelegate");
            ScriptAsDelegate();

            Console.ReadLine();
        }

        public static void EvalWithoutParameters()
        {
            var result = CSharpScript.Eval(@"25 + 30", ScriptOptions.Default);
            Console.WriteLine("Eval: " + result);
        }

        public static void EvalExpression()
        {
            var result = CSharpScript.Eval(@"X + Y", ScriptOptions.Default, new GlobalParams { X = 12, Y = 25 });
            Console.WriteLine("Eval: " + result);
        }

        public static void ScriptCreateAndRun()
        {
            var script = CSharpScript.Create(@"var result = X + Y;", ScriptOptions.Default);
            var state = script.Run(new GlobalParams { X = 10, Y = 2 });
            var state2 = script.Run(new GlobalParams { X = 1, Y = 3 });
            Console.WriteLine("State variable: " + state.Variables["result"].Value);
            Console.WriteLine("State2 variable: " + state2.Variables["result"].Value);
        }

        public static void ScriptWithNamespace()
        {
            var scriptOptions = ScriptOptions.Default.AddNamespaces("System.IO");
            var script = CSharpScript.Create(@"var result = Path.Combine(""folder"", ""file"");", scriptOptions);
            var state = script.Run();
            Console.WriteLine("State variable: " + state.Variables["result"].Value);
        }

        public static void ScriptAsDelegate()
        {
            var script = CSharpScript.Create(@"int Sum(int a, int b) { return a + b;}", ScriptOptions.Default);
            var state = script.Run();
            var sumFunction = state.CreateDelegate<Func<int, int, int>>("Sum");
            Console.WriteLine("Sum function: " + sumFunction(2,22));
        }
    }
}
