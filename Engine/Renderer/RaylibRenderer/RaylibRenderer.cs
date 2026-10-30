using Brutal.Numerics;
using FlashyDJ.Slotmaps;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class RaylibRenderer: IRenderer {
    Raylib_cs.Color bgColor = Raylib_cs.Color.RayWhite;

    private readonly SlotMap<IResource> resources = [];
    private readonly SlotMap<ResourceInstance> instances = [];

    private readonly Dictionary<SlotKey, List<float4x4>> batches = [];

    private Camera3D cam;

    public void Init()
    {
        Raylib.InitWindow(800, 400, "Hello World");

        cam = new()
        {
            FovY = 60.0F,
            Projection = CameraProjection.Perspective,
            Position = new(5.0F, 10.0F, 5.0F),
            Target = new(),
        };
    }

    public void Render()
    {
        Raylib.BeginDrawing();
        {
            Raylib.ClearBackground(bgColor);
            // TODO: Query all rendercomponents, update transforms

            Batch();
            // TODO: multiple cameras?
            Raylib.BeginMode3D(cam);
            {
                foreach (var (rid, xforms) in batches) {
                    /*
                        TODO:
                        1. Get resource
                        2. Draw batched or in a loop at first
                    */
                }
            }
            Raylib.EndMode3D();

            Raylib.DrawText("Hello Raylib!", 12, 12, 20, Raylib_cs.Color.Black);
        }
        Raylib.EndDrawing();
    }

    public void Shutdown()
    {
        Raylib.CloseWindow();
    }

    public SlotKey AddResource(IResource resource)
    {
        // TODO: Error handling
        var key = resources.Add(resource);
        batches[key] = [];
        return key;
    }

    public IResource? GetResource(SlotKey resource) {
        if (!resources.TryGet(resource, out var value)) return null;
        else return value;
    }

    public void DestroyResource(SlotKey handle)
    {
        resources.Remove(handle);
        batches.Remove(handle);
    }

    public SlotKey AddInstance(SlotKey resource, float4x4 transform)
    {
        return instances.Add(new(resource, transform));
    }

    public void DestroyInstance(SlotKey instance) {
        instances.Remove(instance);
    }

    public ResourceInstance? GetInstance(SlotKey instance)
    {
        if (!instances.TryGet(instance, out var value)) return null;
        else return value;
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
            batches[instance.RID].Add(instance.Transform); // If we get an error gete there is an issue with adding lists
        }
    }
}
