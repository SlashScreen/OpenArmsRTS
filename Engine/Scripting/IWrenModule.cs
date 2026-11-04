using WrenSharp;

namespace RTS.Engine.Scripting;

interface IWrenModule {
    public string ModuleSource => string.Join("\n", ClassSources());
    public string ModuleName { get; }

    string[] ClassSources();
    void DefineModuleClasses(WrenSharpVM vm);
}
