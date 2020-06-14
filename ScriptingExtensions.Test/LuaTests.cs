using System;
using NUnit.Framework;
using TCAdmin.SDK.Scripting.Engines;

namespace ScriptingExtensions.Test
{
    [TestFixture]
    public class LuaTests
    {
        [Test]
        public void Test1()
        {
            var javaScriptEngine = new LuaEngine();
            javaScriptEngine.AddVariable("Script", this);
            javaScriptEngine.AddVariable("console", new LuaTests());
            javaScriptEngine.AddVariable("message", "Ooof");

            javaScriptEngine.OutputReceived += Console.WriteLine;

            var script = "Script:WriteToConsole(\"HEYHEYHEY\")";

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