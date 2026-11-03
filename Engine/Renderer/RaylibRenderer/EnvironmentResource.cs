using Brutal.Numerics;
using FlashyDJ.Slotmaps;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

record class EnvironmentResource: IResource {
    Raylib_cs.Color bgColor = Raylib_cs.Color.RayWhite;

    public int RefCount { get; private set; }
    public void Reference() => RefCount++;
    public void Dereference() => RefCount--;

    public ref Camera3D Camera => ref camera;
    public RenderTexture2D RenderTexture => renderTexture;
    Camera3D camera;
    RenderTexture2D renderTexture;

    public EnvironmentResource(Camera3D camera, RenderTexture2D renderTexture) {
        this.camera = camera;
        this.renderTexture = renderTexture;
    }

    public void Draw(IRenderer renderer, Dictionary<SlotKey, List<float4x4>> batches)
    {
        Raylib.BeginTextureMode(renderTexture);
        {
            Raylib.ClearBackground(bgColor);

            Raylib.BeginMode3D(camera);
            foreach (var (rid, xforms) in batches)
            {
                Raylib.DrawGrid(10, 10);

                var res = renderer.GetResource(rid)!;
                switch (res)
                {
                    case IDrawable d:
                        {
                            d.Draw(renderer, xforms);
                            break;
                        }
                }
            }
            Raylib.EndMode3D();
        }
        Raylib.EndTextureMode();
    }

    public void Dispose() {
        Raylib.UnloadTexture(renderTexture.Texture);
    }
}
