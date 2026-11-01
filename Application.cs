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
                rend.AddResource(MaterialResource
                    .CreateFromShaderPaths("resources/shaders/lighting_instancing.vs", "resources/shaders/base.fs")
                    .Apply(m => m.SetMapColor(MaterialMapIndex.Diffuse, Raylib_cs.Color.Blue)))
            )
            );
            var instance1 = rend.AddInstance(modelRid, float4x4.Identity);
            var instance2 = rend.AddInstance(modelRid, float4x4.Identity);

            world.Create(new TransformComponent(), new RenderComponent(instance1), new VeclocityComponent(new float3(1.0F, 0.0F, 1.0F)));
            world.Create(new TransformComponent(), new RenderComponent(instance2), new VeclocityComponent(new float3(0.0F, 1.0F, 0.0F)));
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
