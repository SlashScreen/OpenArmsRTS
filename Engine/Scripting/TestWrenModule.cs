using WrenSharp;

namespace RTS.Engine.Scripting;

class TestWrenModule: IWrenModule {
    public string ModuleName => "test_wren_module";

    public string[] ClassSources() => [
        WrenTestObject.ClassSource()
    ];

    public void DefineModuleClasses(WrenSharpVM vm)
    {
        WrenTestObject.DefineClass(vm, ModuleName);
    }
}
