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

        public class SumParams
        {
            public int a;
            public int b;
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

        public static async void EvalWithoutParameters()
        {
            var result = await CSharpScript.EvaluateAsync(@"25 + 30", ScriptOptions.Default);
            Console.WriteLine("Eval: " + result);
        }

        public static async void EvalExpression()
        {
            var result = await CSharpScript.EvaluateAsync(@"X + Y", ScriptOptions.Default, new GlobalParams { X = 12, Y = 25 });
            Console.WriteLine("Eval: " + result);
        }

        public static async void ScriptCreateAndRun()
        {
            var script = CSharpScript.Create(@"var result = X + Y;", ScriptOptions.Default, typeof(GlobalParams));
            var state = await script.RunAsync(new GlobalParams { X = 10, Y = 2 });
            var state2 = await script.RunAsync(new GlobalParams { X = 1, Y = 3 });
            Console.WriteLine("State variable: " + state.Variables["result"].Value);
            Console.WriteLine("State2 variable: " + state2.Variables["result"].Value);
        }

        public static async void ScriptWithNamespace()
        {
            var scriptOptions = ScriptOptions.Default.AddNamespaces("System.IO");
            var script = CSharpScript.Create(@"var result = Path.Combine(""folder"", ""file"");", scriptOptions);
            var state = await script.RunAsync();
            Console.WriteLine("State variable: " + state.Variables["result"].Value);
        }

        public static async void ScriptAsDelegate()
        {
            var param = new SumParams { a = 2, b = 22 };
            var script = CSharpScript.Create<int>(@"int sum(int a, int b){return a + b;}", ScriptOptions.Default, typeof(SumParams))
                .ContinueWith("sum(a, b)");
            var function = script.CreateDelegate();
            var result = await function(param);
            Console.WriteLine("Sum function: " + result);
        }
    }
}
