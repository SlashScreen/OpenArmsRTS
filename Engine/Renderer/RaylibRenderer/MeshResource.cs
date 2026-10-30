using Brutal.Numerics;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class MeshResource(Mesh mesh) : IResource
{
    public Mesh Mesh => mesh;

    public void Dispose() {
        Raylib.UnloadMesh(mesh);
    }

    public static MeshResource Cube(float3 extents) => new(Raylib.GenMeshCube(extents.X, extents.Y, extents.Z));
    public static MeshResource Sphere(float radius, int rings, int slices) => new(Raylib.GenMeshSphere(radius, rings, slices));
}
