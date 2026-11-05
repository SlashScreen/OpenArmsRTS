using WrenSharp;

namespace RTS.Engine.Scripting;

class WrenModuleProvider(IEnumerable<IWrenModule> wrenModules): IWrenModuleProvider, IWrenModuleResolver {
    readonly Dictionary<string, IWrenModule> modules = wrenModules.ToDictionary(module => module.ModuleName);

    public void RegisterClasses(WrenSharpVM vm) {
        foreach(var (_, mod) in modules) {
            mod.DefineModuleClasses(vm);
        }
    }

    public IWrenSource? GetModuleSource(WrenVM vm, string moduleName)
    {
        if (modules.TryGetValue(moduleName, out var module)) return new WrenStringSource(module.ModuleSource); // yeah it generates this on load but it should only happen once, I think?...
        // TODO: file lookup
        return null;
    }

    public void OnModuleLoadComplete(WrenVM vm, string module, IWrenSource source) => source.Dispose();

    public string ResolveModule(WrenVM vm, string importer, string name) => name; // TODO
}
