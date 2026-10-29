using Microsoft.Extensions.ObjectPool;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class RaylibRenderer: IRenderer {
    Raylib_cs.Color bgColor = Raylib_cs.Color.RayWhite;

    readonly DefaultObjectPool<ResourceHandle> resources = new(new DefaultPooledObjectPolicy<ResourceHandle>());

    public void Init()
    {
        Raylib.InitWindow(800, 400, "Hello World");
    }

    public void Render()
    {
        Raylib.BeginDrawing();
        {
            Raylib.ClearBackground(bgColor);

            Raylib.DrawText("Hello Raylib!", 12, 12, 20, Raylib_cs.Color.Black);
        }
        Raylib.EndDrawing();
    }

    public void Shutdown()
    {
        Raylib.CloseWindow();
    }

    public ResourceHandle AddResource(IResource resource) {
        // TODO: Error handling
        var handle = resources.Get();
        handle.BindResource(resource);
        return handle;
    }

    public void DestroyResource(ResourceHandle handle)
    {
        resources.Return(handle);
    }

    public void DrawResource(ResourceHandle resource)
    {
        // TODO
    }
}
