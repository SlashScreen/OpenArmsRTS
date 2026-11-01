using Brutal.Numerics;
using FlashyDJ.Slotmaps;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class ModelResource : IDrawable
{
    public int RefCount { get; private set; }
    public void Reference() => RefCount++;
    public void Dereference() => RefCount--;

    public SlotKey Mesh { get; init; }
    public SlotKey Material { get; init; }

    public ModelResource(SlotKey mesh, SlotKey material) {
        Mesh = mesh;
        Material = material;

        Engine.Instance.Renderer?.Let((ri) =>
            {
                ri.IncRef(mesh);
                ri.IncRef(material);
            }
        );
    }

    public void Draw(IRenderer renderer, IEnumerable<float4x4> transforms)
    {
        var meshRes = renderer.GetResourceAs<MeshResource>(Mesh) ?? throw new InvalidRidTypeException<MeshResource>(); // Do we move throwing to GetResourceAs?
        var matRes = renderer.GetResourceAs<MaterialResource>(Material) ?? throw new InvalidRidTypeException<MaterialResource>();
        // TODO: Instancing with shader
        foreach (var tx in transforms) {
            Raylib.DrawMesh(meshRes.Mesh, matRes.Material, float4x4.Transpose(tx).ToNumerics());
        }
    }

    public void Dispose() {
        Engine.Instance.Renderer?.Let((ri) =>
        {
            ri.DecRef(Mesh);
            ri.DecRef(Material);
        });
    }
}
