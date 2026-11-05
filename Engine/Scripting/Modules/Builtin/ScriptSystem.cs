using RTS.Engine.Systems;
using WrenSharp;

namespace RTS.Engine.Scripting.Modules.Builtin;

class ScriptSystem: WrenScriptClass, ISystem {
    /*
    TODO
    1. Subscribe to events if they exist
    2. Query, somehow (static method)
   */
    public static new void DefineClass(WrenSharpVM vm, string module) {
        vm
            .Foreign(module, "System")
            .Allocate((WrenVM wvm, ref WrenSharedDataHandle data) =>
            {
                var system = new ScriptSystem();
                data = wvm.SharedData.Add(system);
            })
            .Static("query(_)", ctx => {
                
            });
    }

    public void Init(Engine engine) {
        // TODO: Check if we have events, then subscribe to them
    }
}
