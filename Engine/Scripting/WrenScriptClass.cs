using WrenSharp;

namespace RTS.Engine.Scripting;

abstract class WrenScriptClass: IDisposable {
    public static void DefineClass(WrenSharpVM vm, string module) { }
    public static string ClassSource() => "";

    public void Dispose() {
    }
}
