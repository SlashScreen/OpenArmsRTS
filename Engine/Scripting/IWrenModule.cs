using WrenSharp;

namespace RTS.Engine.Scripting;

interface IWrenModule {
    public IWrenSource ModuleSource { get; }
    public string ModuleName { get; }

    void DefineModuleClasses(WrenSharpVM vm);
}
