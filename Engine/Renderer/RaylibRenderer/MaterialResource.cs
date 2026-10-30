using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class MaterialResource(Material material): IResource {
    public Material Material => material;

    public void Dispose()
    {
        Raylib.UnloadMaterial(material);
    }

    public static MaterialResource CreateDefault() => new(Raylib.LoadMaterialDefault());
}
