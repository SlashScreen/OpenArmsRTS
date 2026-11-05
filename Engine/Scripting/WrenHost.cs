using WrenSharp;

namespace RTS.Engine.Scripting;

/*
    TODO:
    1. Module search
*/

class WrenHost : IDisposable
{
    readonly WrenSharpVM vm;
    readonly WrenModuleProvider moduleProvider;

    public WrenHost(string program, IWrenWriteOutput writeOutput, IWrenErrorOutput errorOutput, IEnumerable<IWrenModule> wrenModules)
    {
        moduleProvider = new(wrenModules);
        var config = new WrenVMConfiguration()
        {
            LogErrors = true,
            ErrorOutput = errorOutput,
            WriteOutput = writeOutput,
            ModuleProvider = moduleProvider,
            ModuleResolver = moduleProvider,
        };
        vm = new(config);
        Interpret(program);
    }

    public WrenHost(string program, IEnumerable<IWrenModule> wrenModules)
    {
        moduleProvider = new(wrenModules);
        var output = new WrenConsoleOutput();
        var config = new WrenVMConfiguration()
        {
            LogErrors = true,
            ErrorOutput = output,
            WriteOutput = output,
            ModuleProvider = moduleProvider,
            ModuleResolver = moduleProvider,
        };
        vm = new(config);
        Interpret(program);
    }

    void Interpret(string program)
    {
        moduleProvider.RegisterClasses(vm);
        vm.Interpret(
            module: "main",
            source: program,
            throwOnFailure: false
        );
    }

    public void Dispose()
    {
        vm.Dispose();
    }
}
