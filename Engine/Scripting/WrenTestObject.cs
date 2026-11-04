using WrenSharp;

namespace RTS.Engine.Scripting;

class WrenTestObject: WrenScriptClass {
    public void SayHi()
    {
        Console.WriteLine("Hello hello!");
    }

    public static new void DefineClass(WrenSharpVM vm, string module)
    {
        vm
            .Foreign(module, "TestClass")
            .Allocate((WrenVM wvm, ref WrenSharedDataHandle data) =>
            {
                var obj = new WrenTestObject();
                data = wvm.SharedData.Add(obj);
            })
            .Instance("sayHi()", ctx =>
            {
                var obj = ctx.GetReceiverSharedData<WrenTestObject>();
                obj.SayHi();
            });
    }

    public static new string ClassSource() => @"
    foreign class TestClass {
        construct new() { }
        foreign sayHi()
    }
    ";
}
