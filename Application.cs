using Brutal.Numerics;
using Raylib_cs;
using RTS.Engine.Components;
using RTS.Engine.Input.RaylibInput;
using RTS.Engine.Renderer.RaylibRenderer;
using RTS.Engine.Scripting;
using RTS.Engine.Systems;
using RTS.Engine.UI;

namespace RTS;

class Application {

    public bool ShouldClose => Raylib.WindowShouldClose() || engine.ShouldShutdown;

    readonly Engine.Engine engine = new(new RaylibRenderer(new ImGuiProvider()), new RaylibInputProvider(new([
        new (new RaylibInputProvider.IsKeyDown(KeyboardKey.W), "forward"),
        new (new RaylibInputProvider.IsKeyDown(KeyboardKey.S), "backward"),
        new (new RaylibInputProvider.IsKeyDown(KeyboardKey.A), "left"),
        new (new RaylibInputProvider.IsKeyDown(KeyboardKey.D), "right"),
        new (new RaylibInputProvider.IsKeyDown(KeyboardKey.Escape), Engine.Engine.QUIT_ACTION_NAME)
    ])));

    readonly WrenHost vm = new(@"
        import ""test_wren_module"" for TestClass
        System.print(""Hello WrenSharp!"")
        var t = TestClass.new()
        t.sayHi()
        ", [new TestWrenModule(),]);

    public void Init()
    {
        engine.Init();

        engine.WithWorld((world) =>
        {
            var rend = engine.Renderer!;
            var modelRid = rend.AddResource(new ModelResource(
                rend.AddResource(MeshResource.Cube(new(1.0F))),
                rend.AddResource(MaterialResource
                    .CreateFromShaderPaths("resources/shaders/lighting_instancing.vs", "resources/shaders/base.fs")
                    .Apply(m => m.SetMapColor(MaterialMapIndex.Diffuse, Raylib_cs.Color.Blue)))
            )
            );
            var instance1 = rend.AddInstance(modelRid, float4x4.Identity);
            var instance2 = rend.AddInstance(modelRid, float4x4.Identity);

            world.Create(new TransformComponent(), new RenderComponent(instance1), new TargetComponent(new float3(10.0F, 0.0F, 1.0F)), new MobileComponent(1.0F));
            world.Create(new TransformComponent(), new RenderComponent(instance2), new TargetComponent(new float3(5.0F, 0.0F, 5.0F)), new MobileComponent(1.5F));

            var env = rend.GetResourceAs<EnvironmentResource>(rend.MainEnvironment) ?? throw new Exception("girl what");
            var cam_pos = env.Camera.Position.ToBrutal();
            world.Create(new TransformComponent(float4x4.CreateTranslation(cam_pos)), new CameraComponent(rend.MainEnvironment));
        });

        engine.AddSystem(new CameraInputSystem());
        engine.AddSystem(new MoveToTargetSystem());
        engine.AddSystem(new RenderTransformUpdateSystem());
    }

    public void Run()
    {
        while (!ShouldClose) {
            engine.Tick(Raylib.GetFrameTime()); // TODO: Some other way of doing this
        }
    }

    public void Shutdown() {
        vm.Dispose();
        engine.Dispose();
    }
}
