using Arch.Core;
using Arch.Core.Events;
using RTS.Engine.Input;
using RTS.Engine.Input.RaylibInput;
using RTS.Engine.Renderer;
using RTS.Engine.Systems;

namespace RTS.Engine;

class Engine: IDisposable {
    public delegate void SystemDelegate(World world);
    public delegate void SystemTickDelegate(World world, double delta);
    public delegate bool SystemInputDelegate(World world, InputEvent inputEvent, double delta);

    public const int TPS = 20;
    public const double PHYSICS_DELTA = 1.0 / TPS;
    public const string QUIT_ACTION_NAME = "quit";

    static Engine? instance;
    public static Engine Instance => instance ?? throw new Exception("Engine not initialized.");

    event SystemTickDelegate? OnTick;
    event SystemDelegate? OnPhysicsTick;
    Dictionary<string,  List<SystemInputDelegate>> onInputEvent = [];

    public bool ShouldShutdown { get; private set; }
    public IRenderer? Renderer;
    public VFS.VFS VFS { get; init; }
    public IInputProvider? InputProvider { get; init; }

    private World? ecsWorld;

    public Engine(IRenderer? renderer, IInputProvider? inputProvider, List<string> loadOrder)
    {
        instance = this;
        Renderer = renderer;
        VFS = new(loadOrder);
        InputProvider = inputProvider;

        InputProvider?.Let(ip =>
        {
            ip.OnInput += OnInput;
        });
    }

    public Engine(IRenderer? renderer, IInputProvider? inputProvider) : this(renderer, inputProvider, []) { }

    public Engine() : this(null, null, []) { }

    public void Init()
    {
        ecsWorld = World.Create();
        Renderer?.Init();
    }

    public void Tick(double delta)
    {
        InputProvider?.Poll(delta);
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

    public void SubscribeOnInputEvent(string eventName, SystemInputDelegate fn) => onInputEvent.GetOrCreate(eventName).Add(fn);

    void OnInput(string name, InputEvent inputEvent, double delta) {
        // TODO: Not hardcode this
        switch (name) {
            case QUIT_ACTION_NAME:
                if (inputEvent is RaylibInputProvider.KeyDown)
                    RequestShutdown();
                break;
            default:
                // Run queue, stopping if any input system consumes the action
                if (onInputEvent.TryGetValue(name, out var callbacks)) {
                    foreach (var callback in callbacks) {
                        if (callback(ecsWorld!, inputEvent, delta)) break;
                    }
                }
                break;
        }
    }
}
