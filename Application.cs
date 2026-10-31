using Brutal.Numerics;
using Raylib_cs;
using RTS.Engine.Components;
using RTS.Engine.Renderer.RaylibRenderer;
using RTS.Engine.Systems;

namespace RTS;

class Application {

    public bool ShouldClose => Raylib.WindowShouldClose() || engine.ShouldShutdown;

    readonly Engine.Engine engine = new(new RaylibRenderer());

    public void Init()
    {
        engine.Init();

        engine.WithWorld((world) =>
        {
            var rend = engine.Renderer!;
            var modelRid = rend.AddResource(new ModelResource(
                rend.AddResource(MeshResource.Cube(new(1.0F))),
                rend.AddResource(MaterialResource.CreateWithColor(Raylib_cs.Color.Blue)))
            );
            var instance = rend.AddInstance(modelRid, float4x4.Identity);

            world.Create(new TransformComponent(), new RenderComponent(instance), new VeclocityComponent(new float3(1.0F, 0.0F, 1.0F)));
        });

        engine.AddSystem(new TestSystem());
        engine.AddSystem(new RenderTransformUpdateSystem());
    }

    public void Run()
    {
        while (!ShouldClose) {
            engine.Tick(Raylib.GetFrameTime()); // TODO: Some other way of doing this
        }
    }

    public void Shutdown() {
        engine.Dispose();
    }
}
