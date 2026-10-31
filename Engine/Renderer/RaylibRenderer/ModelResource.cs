using System.Numerics;
using Brutal.Numerics;
using FlashyDJ.Slotmaps;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class ModelResource(SlotKey mesh, SlotKey material) : IDrawable
{
    public SlotKey Mesh
    {
        get => mesh;
        set => mesh = value;
    }
    public SlotKey Material
    {
        get => material;
        set => material = value;
    }

    public void Draw(IRenderer renderer, IEnumerable<float4x4> transforms)
    {
        var meshRes = renderer.GetResourceAs<MeshResource>(mesh) ?? throw new InvalidRidTypeException<MeshResource>(); // Do we move throwing to GetResourceAs?
        var matRes = renderer.GetResourceAs<MaterialResource>(material) ?? throw new InvalidRidTypeException<MaterialResource>();
        // TODO: Instancing with shader
        foreach (var tx in transforms) {
            Raylib.DrawMesh(meshRes.Mesh, matRes.Material, float4x4.Transpose(tx).ToNumerics());
        }
    }

    public void Dispose() { }
}
