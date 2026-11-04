using RTS.Engine.Systems;
using WrenSharp;

namespace RTS.Engine.Scripting;

// owns vm
class WrenSystem(WrenVM vm, WrenHandle classHandle)
{
    public static void DefineClass(WrenSharpVM svm)
    {
        svm
            .Foreign("ecs", "System")
            // TODO: Allocate and finalize
            .Instance("query", ctx => {
                // TODO: Class list and query
                ctx.Return(0);
            });
    }

    // public void Init(Engine engine) {
    //     using var initCallHandle = vm.CreateCallHandle("init()");
    //     using var call = vm.CreateCall(Handle, initCallHandle);
    //     call.Call();
    // }
}
