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
                var arg = ctx.GetArgInt32(0);
                Console.Write($"Arg is {arg}");
            });
    }
}
