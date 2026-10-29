namespace RTS.Engine.Renderer;

interface IRenderer {
    void Init();

    void Render();

    void Shutdown();

    ResourceHandle AddResource(IResource resource);

    void DestroyResource(ResourceHandle handle);

    void DrawResource(ResourceHandle resource);
}
