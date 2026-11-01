using FlashyDJ.Slotmaps;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class MaterialResource(Material material) : IResource
{
    public Material Material => material;

    public int RefCount { get; private set; }
    public void Reference() => RefCount++;
    public void Dereference() => RefCount--;

    public SlotKey? Shader
    {
        get; set
        {
            field?.Let(f => Engine.Instance.Renderer?.DecRef(f)); // Deref old if it is not null
            value?.Let(v => // If setting to not null, Ref
            {
                Engine.Instance.Renderer?.Let(ri =>
                {
                    ri.IncRef(v);
                    var sh = ri.GetResourceAs<ShaderResource>(v) ?? throw new InvalidRidTypeException<ShaderResource>();
                    material.Shader = sh.Shader;
                });
            });

            field = value;
        }
    }

    readonly SlotKey?[] textures = new SlotKey?[Enum.GetValues(typeof(MaterialMapIndex)).Length];

    ref MaterialMap GetMap(MaterialMapIndex mapIndex)
    {
        unsafe
        {
            return ref material.Maps[(int)mapIndex];
        }
    }

    public void SetMapColor(MaterialMapIndex mapIndex, Color color)
    {
        GetMap(mapIndex).Color = color;
    }

    public void SetMapTexture(MaterialMapIndex mapIndex, SlotKey texture)
    {
        var ri = Engine.Instance.Renderer ?? throw new Exception("No renderer.");
        textures[(int)mapIndex]?.Let(ri.DecRef);

        var texRes = ri.GetResourceAs<Texture2DResource>(texture) ?? throw new InvalidRidTypeException<Texture2DResource>();
        textures[(int)mapIndex] = texture;
        ri.IncRef(texture);

        Raylib.SetMaterialTexture(ref material, mapIndex, texRes.Texture);
    }

    public void Dispose()
    {
        Raylib.UnloadMaterial(material);

        var ri = Engine.Instance.Renderer ?? throw new Exception("No renderer.");
        foreach(var tex in textures) {
            tex?.Let(ri.DecRef);
        }
        Shader = null; // Will deref
    }

    public static MaterialResource CreateDefault() => new(Raylib.LoadMaterialDefault());
    public static MaterialResource CreateWithColor(Color color) => new MaterialResource(Raylib.LoadMaterialDefault()).Apply((m) => m.SetMapColor(MaterialMapIndex.Diffuse, color));
    public static MaterialResource CreateFromShaderData(string vertShaderData, string fragShaderData)
    {
        var shader = new ShaderResource(Raylib.LoadShaderFromMemory(vertShaderData, fragShaderData));

        return new(Raylib.LoadMaterialDefault())
        {
            Shader = Engine.Instance.Renderer?.AddResource(shader)
        };
    }
    public static MaterialResource CreateFromShaderPaths(string vertPath, string fragPath) {
        // Resolve path
        if (!Engine.Instance.VFS.TryResolvePath(vertPath, out var resolvedVs)) throw new Exception($"Could not find vertex shader at path {vertPath}");
        if (!Engine.Instance.VFS.TryResolvePath(fragPath, out var resolvedFs)) throw new Exception($"Could not find fragment shader at path {fragPath}");
        // Load data
        if (!Engine.Instance.VFS.TryReadFileResolved(resolvedVs, out string vertData)) throw new Exception($"Could not read vertex shader file {vertPath}");
        if (!Engine.Instance.VFS.TryReadFileResolved(resolvedFs, out string fragData)) throw new Exception($"Could not read fragment shader file {fragPath}");

        return CreateFromShaderData(vertData, fragData);
    }
}
