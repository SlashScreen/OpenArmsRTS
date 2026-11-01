using Arch.Core;
using Arch.Core.Events;
using Raylib_cs;
using RTS.Engine.Renderer;
using RTS.Engine.Systems;

namespace RTS.Engine;

class Engine: IDisposable {
    public delegate void SystemDelegate(World world);
    public delegate void SystemTickDelegate(World world, double delta);

    public const int TPS = 20;
    public const double PHYSICS_DELTA = 1.0 / TPS;

    static Engine? instance;
    public static Engine Instance => instance ?? throw new Exception("Engine not initialized.");

    event SystemTickDelegate? OnTick;
    event SystemDelegate? OnPhysicsTick;

    public bool ShouldShutdown { get => field || Raylib.WindowShouldClose(); private set; } // TODO: Some other way of doing this for input
    public IRenderer? Renderer;

    private World? ecsWorld;

    public Engine(IRenderer? renderer) {
        instance = this;
        Renderer = renderer;
    }

    public void Init()
    {
        ecsWorld = World.Create();
        Renderer?.Init();
    }

    public void Tick(double delta)
    {
        OnTick?.Invoke(ecsWorld!, delta);
        Renderer?.Render();
    }

    public void PhysicsTick() {
        OnPhysicsTick?.Invoke(ecsWorld!);
    }

    public void Dispose()
    {
        Renderer?.Shutdown();
        ecsWorld!.Dispose();
    }

    public void RequestShutdown() => ShouldShutdown = true;

    public void AddSystem(ISystem system) => system.Init(this); // TODO: Management

    public void SubscribeTick(SystemTickDelegate fn)
    {
        OnTick += fn;
    }

    public void SubscribePhysicsTick(SystemDelegate fn)
    {
        OnPhysicsTick += fn;
    }

    public void WithWorld(SystemDelegate fn) => fn(ecsWorld!);

    public void DestroyEntity(Entity entity) => ecsWorld!.Destroy(entity);

    public void SubscribeEntityCreated(EntityCreatedHandler fn) => ecsWorld!.SubscribeEntityCreated(fn);

    public void SubscribeEntityDestroyed(EntityDestroyedHandler fn) => ecsWorld!.SubscribeEntityDestroyed(fn);

    public void SubscribeComponentAdded<T>(ComponentAddedHandler<T> fn) => ecsWorld!.SubscribeComponentAdded<T>(fn);

    public void SubscribeComponentSet<T>(ComponentSetHandler<T> fn) => ecsWorld!.SubscribeComponentSet<T>(fn);

    public void SubscribeComponentRemoved<T>(ComponentRemovedHandler<T> fn) => ecsWorld!.SubscribeComponentRemoved<T>(fn);
}
