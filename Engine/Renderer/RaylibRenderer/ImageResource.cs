using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class ImageResouce(Image image): IResource {
    public Image Image => image;

    public int RefCount { get; private set; } = 1;
    public void Reference() => RefCount++;
    public void Dereference() => RefCount--;

    public void Dispose()
    {
        Raylib.UnloadImage(image);
    }
}
