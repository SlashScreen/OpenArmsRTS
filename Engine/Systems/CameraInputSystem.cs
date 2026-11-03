using Arch.Core;
using Brutal.Numerics;
using RTS.Engine.Components;
using RTS.Engine.Input;
using RTS.Engine.Input.RaylibInput;
using RTS.Engine.Renderer;
using RTS.Engine.Renderer.RaylibRenderer;

namespace RTS.Engine.Systems;

class CameraInputSystem: ISystem {
    const float MOVE_SPEED = 5.0F; // TODO: Move to component or make it zoom dependent
    readonly QueryDescription query = new QueryDescription().WithAll<TransformComponent, CameraComponent>();

    public void Init(Engine engine)
    {
        engine.SubscribeOnInputEvent("forward", OnForward);
        engine.SubscribeOnInputEvent("backward", OnBackward);
        engine.SubscribeOnInputEvent("left", OnLeft);
        engine.SubscribeOnInputEvent("right", OnRight);
    }

    bool OnForward(World world, InputEvent inputEvent, double delta)=> OnMove(world, inputEvent, delta, new(0.0F, 0.0F, -1.0F), "Forward!");

    bool OnBackward(World world, InputEvent inputEvent, double delta) => OnMove(world, inputEvent, delta, new(0.0F, 0.0F, 1.0F), "Back!");

    bool OnLeft(World world, InputEvent inputEvent, double delta) => OnMove(world, inputEvent, delta, new(-1.0F, 0.0F, 0.0F), "Left!");

    bool OnRight(World world, InputEvent inputEvent, double delta) => OnMove(world, inputEvent, delta, new(1.0F, 0.0F, 0.0F), "Right!");

    bool OnMove(World world, InputEvent inputEvent, double delta, float3 direction, string debugMessage) {
        if (inputEvent is RaylibInputProvider.KeyPressed)
        {
            Console.WriteLine(debugMessage);
            return true;
        }
        else if (inputEvent is RaylibInputProvider.KeyDown)
        {
            world.Query(in query, (Entity entity, ref TransformComponent tx, ref CameraComponent camComponent) =>
            {
                var env = Engine.Instance.Renderer?.GetResourceAs<EnvironmentResource>(camComponent.CamEnvironment) ?? throw new InvalidRidTypeException<EnvironmentResource>();
                tx.Position += direction * MOVE_SPEED * (float)delta;
                env.Camera.Position = tx.Position.ToNumerics();
                // This tilts it down at a 45 degree angle... I think.
                env.Camera.Target = new float3(tx.Position.X, 0.0F, tx.Position.Z - tx.Position.Y).ToNumerics();
            });
            return true;
        }
        else
        {
            return false;
        }
    }
}
