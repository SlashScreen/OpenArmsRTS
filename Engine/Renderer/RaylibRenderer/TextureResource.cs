using FlashyDJ.Slotmaps;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class Texture2DResource(Texture2D texture): IResource {
    public Texture2D Texture => texture;

    public int RefCount { get; private set; }
    public void Reference() => RefCount++;
    public void Dereference() => RefCount--;

    public void Dispose()
    {
        Raylib.UnloadTexture(texture);
    }

    public static Texture2DResource LoadFromImage(SlotKey image) {
        var ri = Engine.Instance.Renderer ?? throw new Exception("No renderer.");
        return new(Raylib.LoadTextureFromImage(ri.GetResourceAs<ImageResource>(image)?.Image ?? throw new InvalidRidTypeException<ImageResource>()));
    }
}
