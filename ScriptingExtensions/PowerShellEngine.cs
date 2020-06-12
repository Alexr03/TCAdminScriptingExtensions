// using System;
// using System.Collections.Generic;
// using System.IO;
// using TCAdmin.Interfaces.Logging;
// using TCAdmin.SDK.Security;
//
// namespace TCAdmin.SDK.Scripting.Engines
// {
//
//     public class PowerShellEngine : IScriptingEngine
//     {
//         private string _script;
//         private readonly Dictionary<string, object> _variables;
//
//         public PowerShellEngine()
//         {
//             _script = "";
//             _variables = new Dictionary<string, object>();
//             
//             this.AddVariable("Script", this);
//             this.AddVariable("TCAdminFolder", Path.GetFullPath(Path.Combine(Path.Combine(Utility.GetSharedPath(), ".."), "..")));
//         }
//         
//         public void AddVariable(string name, object value)
//         {
//             name = name.Replace(".", "_");
//             this._variables.Add(name, value);
//         }
//
//         public object GetVariable(string name)
//         {
//             return this._variables.TryGetValue(name, out var value) ? value : null;
//         }
//
//         public void SetScript(string script)
//         {
//             _script = script.TrimStart('\n', '\r');
//         }
//
//         public int Execute(Credentials credentials)
//         {
//             try
//             {
//                 using (var powerShellInstance = PowerShell.Create())
//                 {
//                     powerShellInstance.AddScript(_script);
//                     
//                     foreach (var variable in _variables)
//                     {
//                         powerShellInstance.AddParameter(variable.Key, variable.Value);
//                     }
//                     
//                     foreach (var outputItem in powerShellInstance.Invoke())
//                     {
//                         if (outputItem != null)
//                         {
//                             WriteToConsole(outputItem.BaseObject.ToString());
//                         }
//                     }
//
//                     if (powerShellInstance.Streams.Error.Count <= 0) return 0;
//                     foreach (var error in powerShellInstance.Streams.Error)
//                     {
//                         WriteToConsole(error.ToString());
//                     }
//
//                     return 0;
//                 }
//             }
//             catch (Exception e)
//             {
//                 WriteToConsole("Exception");
//                 WriteToConsole(e.Message);
//                 return -1;
//             }
//         }
//
//         public void CheckSyntax(string script)
//         {
//             // To do
//         }
//         
//         public void WriteToConsole(string output)
//         {
//             LogManager.Write(output, LogType.Console);
//             IScriptingEngine.OutputReceivedEventHandler outputReceivedEvent = this.OutputReceived;
//             outputReceivedEvent?.Invoke(output);
//         }
//
//         public event IScriptingEngine.OutputReceivedEventHandler OutputReceived;
//     }
// }