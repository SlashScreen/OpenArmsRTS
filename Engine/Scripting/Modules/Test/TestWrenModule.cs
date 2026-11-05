using WrenSharp;

namespace RTS.Engine.Scripting.Modules.Test;

class TestWrenModule: IWrenModule {
    public IWrenSource ModuleSource => new WrenStringSource(string.Join("\n", ClassSources()));
    public string ModuleName => "test_wren_module";

    public string[] ClassSources() => [
        WrenTestObject.ClassSource()
    ];

    public void DefineModuleClasses(WrenSharpVM vm)
    {
        WrenTestObject.DefineClass(vm, ModuleName);
    }
}
