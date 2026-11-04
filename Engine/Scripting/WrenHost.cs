using WrenSharp;

namespace RTS.Engine.Scripting;

class WrenHost : IDisposable
{
    readonly WrenSharpVM vm;

    public WrenHost(string program, IWrenWriteOutput writeOutput, IWrenErrorOutput errorOutput)
    {
        var config = new WrenVMConfiguration()
        {
            LogErrors = true,
            ErrorOutput = errorOutput,
            WriteOutput = writeOutput,
        };
        vm = new(config);
        Interpret(program);
    }

    public WrenHost(string program)
    {
        var output = new WrenConsoleOutput();
        var config = new WrenVMConfiguration()
        {
            LogErrors = true,
            ErrorOutput = output,
            WriteOutput = output,
        };
        vm = new(config);
        Interpret(program);
    }

    void Interpret(string program)
    {
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
