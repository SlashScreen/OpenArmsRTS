using Arch.Core;
using RTS.Engine.Components;
using RTS.Engine.Renderer;

namespace RTS.Engine.Systems;

/// <summary> System for updating an instance's transform. </summary>
class RenderTransformUpdateSystem() : ISystem
{
    readonly QueryDescription query = new QueryDescription().WithAll<TransformComponent, RenderComponent>();

    IRenderer? renderer;

    public void Init(Engine engine)
    {
        engine.Renderer.Let((r) =>
        {
            renderer = r;
            engine.SubscribeTick(OnTick);
        });
    }

    void OnTick(World world, double _delta)
    {
        world.Query(in query, (Entity entity, ref TransformComponent tx, ref RenderComponent ren) =>
        {
            var instance = renderer!.GetInstance(ren.Instance) ?? throw new Exception("Invalid RID");
            instance.Transform = tx.Xform;
        });
    }
}
