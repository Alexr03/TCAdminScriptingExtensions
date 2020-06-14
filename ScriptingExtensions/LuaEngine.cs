using System;
using System.IO;
using Neo.IronLua;
using TCAdmin.Interfaces.Logging;
using TCAdmin.SDK.Security;

namespace TCAdmin.SDK.Scripting.Engines
{
    public class LuaEngine : IScriptingEngine
    {
        private string _script;
        private readonly Lua _lua;
        private readonly LuaGlobal _luaEnvironment;

        public LuaEngine()
        {
            this._script = "";
            
            _lua = new Lua();
            _luaEnvironment = _lua.CreateEnvironment();
            
            this.AddVariable("TCAdminFolder", Path.GetFullPath(Path.Combine(Path.Combine(Utility.GetSharedPath(), ".."), "..")));
            this.AddVariable("Script", this);
        }
        
        public void AddVariable(string name, object value)
        {
            name = name.Replace(".", "_");
            _luaEnvironment.SetMemberValue(name, value);
        }

        public object GetVariable(string name)
        {
            name = name.Replace(".", "_");
            return _luaEnvironment.GetMemberValue(name);
        }

        public void SetScript(string script)
        {
            _script = script.TrimStart('\n', '\r');
        }

        public int Execute(Credentials credentials)
        {
            try
            {
                _luaEnvironment.DoChunk(_script, "TCAScript", null);

                _luaEnvironment.Clear();
                _lua.Dispose();

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
            throw new System.NotImplementedException();
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