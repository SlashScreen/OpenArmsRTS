using Arch.Core;
using RTS.Engine.Components;

namespace RTS.Engine.Systems;

class TestSystem: ISystem {
    readonly QueryDescription query = new QueryDescription().WithAll<TransformComponent, VeclocityComponent>();

    public void Init(Engine engine)
    {
        engine.SubscribeTick(OnTick);
    }

    void OnTick(World world, double delta) {
        world.Query(in query, (Entity entity, ref TransformComponent tx, ref VeclocityComponent vel) =>
        {
            tx.Position += vel.Delta * (float)delta;
            //Console.WriteLine($"{tx.Position} ({(float)delta})");
        });
    }
}
