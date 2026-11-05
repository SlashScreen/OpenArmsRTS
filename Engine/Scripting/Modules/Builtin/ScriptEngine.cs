using WrenSharp;

namespace RTS.Engine.Scripting.Modules.Builtin;

class ScriptEngine: WrenScriptClass {
    // See builtin/resources/modules/builtin.wren for source
    public static new void DefineClass(WrenSharpVM vm, string module)
    {
        vm
            .Foreign(module, "Engine")
            .Static("testMe()", ctx => {
                Console.WriteLine("Engine static test");
            })
            .Static("addSystem(_)", ctx => {
                if (!ctx.TryGetArgSharedData<ScriptSystem>(0, out var arg)) throw new Exception("Arg 0 for addSystem must be builtin.System");
                Engine.Instance.AddSystem(arg);
                Console.Write($"Added system");
            });
    }
}
