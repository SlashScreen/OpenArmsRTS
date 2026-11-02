using Brutal.Numerics;
using FlashyDJ.Slotmaps;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

record class EnvironmentResource(Camera3D Camera, RenderTexture2D RenderTexture): IResource {
    Raylib_cs.Color bgColor = Raylib_cs.Color.RayWhite;

    public int RefCount { get; private set; }
    public void Reference() => RefCount++;
    public void Dereference() => RefCount--;

    public void Draw(IRenderer renderer, Dictionary<SlotKey, List<float4x4>> batches)
    {
        Raylib.BeginTextureMode(RenderTexture);
        {
            Raylib.ClearBackground(bgColor);

            Raylib.BeginMode3D(Camera);
            foreach (var (rid, xforms) in batches)
            {
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
        Raylib.UnloadTexture(RenderTexture.Texture);
    }
}
