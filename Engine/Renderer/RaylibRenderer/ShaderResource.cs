using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class ShaderResource(Shader shader) : IResource
{
    public Shader Shader => shader;

    public int RefCount { get; private set; }
    public void Reference() => RefCount++;
    public void Dereference() => RefCount--;

    public void Dispose()
    {
        Raylib.UnloadShader(shader);
    }
}
