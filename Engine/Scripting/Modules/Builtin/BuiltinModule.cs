using WrenSharp;

namespace RTS.Engine.Scripting.Modules.Builtin; // christ


class BuiltinModule: IWrenModule {
    static readonly string SOURCE_PATH = "builtin/resources/modules/builtin.wren"; // We don't want users overriding this so we bake it

    public IWrenSource ModuleSource => new WrenFileSource(SOURCE_PATH);
    public string ModuleName => "builtin";

    public void DefineModuleClasses(WrenSharpVM vm) {
        ScriptEngine.DefineClass(vm, ModuleName);
    }
}
