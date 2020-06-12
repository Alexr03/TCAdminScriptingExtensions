using System;
using System.IO;
using Jint;
using TCAdmin.Interfaces.Logging;
using TCAdmin.SDK.Security;

namespace TCAdmin.SDK.Scripting.Engines
{
    public class JavaScriptEngine : IScriptingEngine
    {
        private string _script;
        private readonly Engine _javascriptContext;        
        
        public JavaScriptEngine()
        {
            this._script = "";
            _javascriptContext = new Engine(cfg => cfg.AllowClr());
            
            this.AddVariable("TCAdminFolder", Path.GetFullPath(Path.Combine(Path.Combine(Utility.GetSharedPath(), ".."), "..")));
            this.AddVariable("Script", this);
        }

        public void AddVariable(string name, object value)
        {
            name = name.Replace(".", "_");
            _javascriptContext.SetValue(name, value);
        }

        public object GetVariable(string name)
        {
            return _javascriptContext.GetValue(name).ToObject();
        }

        public void SetScript(string script)
        {
            _script = script.TrimStart('\n', '\r');
        }

        public int Execute(Credentials credentials)
        {
            try
            {
                _javascriptContext.Execute(_script);
                return 0;
            }
            catch (Exception e)
            {
                WriteToConsole("Exception!");
                WriteToConsole(e.Message);
                
                return -1;
            }
        }

        public void CheckSyntax(string script)
        {
            // throw new System.NotImplementedException();
        }
        
        public void WriteToConsole(string output)
        {
            LogManager.Write(output, LogType.Console);
            IScriptingEngine.OutputReceivedEventHandler outputReceivedEvent = this.OutputReceived;
            outputReceivedEvent?.Invoke(output);
        }

        public event IScriptingEngine.OutputReceivedEventHandler OutputReceived;
    }
}