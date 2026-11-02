using Brutal.Numerics;
using FlashyDJ.Slotmaps;
using Raylib_cs;
using RTS.Engine.UI;

namespace RTS.Engine.Renderer.RaylibRenderer;

class RaylibRenderer(IUIProvider? uiProvider): IRenderer {
    Raylib_cs.Color bgColor = Raylib_cs.Color.RayWhite;

    private readonly SlotMap<IResource> resources = [];
    private readonly SlotMap<ResourceInstance> instances = [];

    private readonly Dictionary<SlotKey, List<float4x4>> batches = [];

    private Camera3D cam;
    private RenderTexture2D frameBuffer;

    public RaylibRenderer() : this(null) { }

    public void Init()
    {
        Raylib.InitWindow(800, 400, "Hello World");

        cam = new()
        {
            FovY = 45.0F,
            Projection = CameraProjection.Perspective,
            Position = new(0.0F, 10.0F, 10.0F),
            Target = new(0.0F),
            Up = new(0.0F, 1.0F, 0.0F),
        };

        Raylib.SetTargetFPS(60);

        uiProvider?.Init();

        frameBuffer = Raylib.LoadRenderTexture(Raylib.GetRenderWidth(), Raylib.GetRenderHeight());
    }

    public void Render()
    {
        Raylib.BeginTextureMode(frameBuffer);
        {
            Raylib.ClearBackground(bgColor);
            // TODO: Query all rendercomponents, update transforms

            Batch();
            // TODO: multiple cameras?
            Raylib.BeginMode3D(cam);
            {
                Raylib.DrawCubeV(new(0.0F, 3.0F, 0.0F), new(1.0F), Raylib_cs.Color.Red);
                //Raylib.DrawGrid(10, 10);

                foreach (var (rid, xforms) in batches) {
                    var res = GetResource(rid)!;
                    switch (res) {
                        case IDrawable d: {
                                d.Draw(this, xforms);
                                break;
                        }
                    }
                }
            }
            Raylib.EndMode3D();

            uiProvider?.Frame();
        }
        Raylib.EndTextureMode();
        // draw framebuffer
        Raylib.BeginDrawing();
        {
            var w = Raylib.GetRenderWidth();
            var h = Raylib.GetRenderHeight();

            Raylib.DrawTextureRec(frameBuffer.Texture, new(new(0.0F), w, -h), new(0.0F), Raylib_cs.Color.White);
        }
        Raylib.EndDrawing();
    }

    public void Shutdown()
    {
        foreach (var (rid, _) in instances)
        {
            DestroyInstance(rid);
        }

        resources.Clear();
        uiProvider?.Dispose();
        Raylib.UnloadTexture(frameBuffer.Texture);
        Raylib.CloseWindow();
    }

    public SlotKey AddResource(IResource resource)
    {
        // TODO: Error handling
        var key = resources.Add(resource);
        batches[key] = [];
        return key;
    }

    public IResource? GetResource(SlotKey resource)
    {
        if (!resources.TryGet(resource, out var value)) return null;
        else return value;
    }

    public T? GetResourceAs<T>(SlotKey resource) where T : IResource => GetResource(resource) switch
    {
        T val => val,
        _ => default,
    };

    public void DestroyResource(SlotKey handle)
    {
        resources.Remove(handle).Dispose();
        batches.Remove(handle);
    }

    public SlotKey AddInstance(SlotKey resource, float4x4 transform)
    {
        return instances.Add(new(resource, transform));
    }

    public void DestroyInstance(SlotKey instance) {
        instances.Remove(instance).Dispose();
    }

    public ResourceInstance? GetInstance(SlotKey instance)
    {
        if (!instances.TryGet(instance, out var value)) return null;
        else return value;
    }

    public void IncRef(SlotKey rid) => GetResource(rid)?.Reference();

    public void DecRef(SlotKey rid)
    {
        GetResource(rid).Let(res =>
        {
            res.Dereference();

            if (res.RefCount <= 0)
            {
                Console.WriteLine($"Resource {rid} is unreferenced. Unloading...");
                DestroyResource(rid);
            }
        });
    }

    void Batch() {
        // Clear batch lists
        foreach (var (_, value) in batches)
        {
            value.Clear();
        }

        // Instance transforms list
        foreach (var (_, instance) in instances)
        {
            if (instance == null) continue;
            //Console.WriteLine($"batching transform {instance.Transform}");
            batches[instance.RID].Add(instance.Transform); // If we get an error here there is an issue with adding lists
        }
    }
}
