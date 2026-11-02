using Arch.Core;
using Brutal.Numerics;
using RTS.Engine.Components;

namespace RTS.Engine.Systems;

// TODO: Replace with flow field following system
class MoveToTargetSystem : ISystem
{
    readonly QueryDescription query = new QueryDescription().WithAll<TransformComponent, MobileComponent, TargetComponent>();

    const float TARGET_MIN_DIST = 0.01F;

    public void Init(Engine engine)
    {
        engine.SubscribeTick(OnTick);
    }

    void OnTick(World world, double delta)
    {
        world.Query(in query, (Entity entity, ref TransformComponent tx, ref MobileComponent mob, ref TargetComponent target) =>
        {
            // TODO: If FPS is too low it will jitter
            var dist = float3.Distance(tx.Position, target.MovementTarget);

            if (dist < float.Epsilon)
            { // if same remove target
                world.Remove<TargetComponent>(entity);
            }
            else if (dist < TARGET_MIN_DIST)
            { // if close set position and remove target.
                tx.Position = target.MovementTarget;
                world.Remove<TargetComponent>(entity);
            }
            else
            { // else move towards target
                var dir = float3.Normalize(target.MovementTarget - tx.Position);
                tx.Position += dir * mob.MoveSpeed * (float)delta;
            }
        });
    }
}
