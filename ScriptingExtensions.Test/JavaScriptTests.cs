using System;
using NUnit.Framework;
using TCAdmin.SDK.Scripting.Engines;

namespace ScriptingExtensions.Test
{
    [TestFixture]
    public class JavaScriptTests
    {
        [Test]
        public void Test1()
        {
            var javaScriptEngine = new JavaScriptEngine();
            javaScriptEngine.AddVariable("Script", this);
            javaScriptEngine.AddVariable("console", new JavaScriptTests());
            javaScriptEngine.AddVariable("message", "Ooof");

            javaScriptEngine.OutputReceived += Console.WriteLine;

            var script = @"

function test() {
    Script.WriteToConsole('TESTHELLO');
}

test();

";

            javaScriptEngine.SetScript(script);

            var t = javaScriptEngine.Execute(null);
            Console.WriteLine("T = " + t);
            Assert.True(true);
        }

        public void WriteToConsole(string text)
        {
            Console.WriteLine(text);
        }
    }
}