using Brutal.Numerics;
using FlashyDJ.Slotmaps;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class MeshResource(Mesh mesh) : IResource
{
    public Mesh Mesh => mesh;

    public int RefCount { get; private set; }
    public void Reference() => RefCount++;
    public void Dereference() => RefCount--;

    public void Dispose()
    {
        Raylib.UnloadMesh(mesh);
    }

    public static MeshResource Cube(float3 extents) => new(Raylib.GenMeshCube(extents.X, extents.Y, extents.Z));
    public static MeshResource Sphere(float radius, int rings, int slices) => new(Raylib.GenMeshSphere(radius, rings, slices));
    public static MeshResource Heightmap(SlotKey heightmap, float3 size) => new(
        Raylib.GenMeshHeightmap(
            Engine.Instance.Renderer?.GetResourceAs<ImageResouce>(heightmap)?.Image ?? throw new InvalidRidTypeException<ImageResouce>(),
            size.ToNumerics()
        )
    );
}
