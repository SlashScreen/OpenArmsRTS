using WrenSharp;

namespace RTS.Engine.Scripting;

// TODO: when move to C#11, change to interface
abstract class WrenScriptClass : IDisposable
{
    public static void DefineClass(WrenSharpVM vm, string module) { }
    public static string ClassSource() => "";

    public void Dispose()
    {
    }
}
