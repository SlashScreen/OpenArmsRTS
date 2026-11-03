using Brutal.Numerics;
using FlashyDJ.Slotmaps;

namespace RTS.Engine.Renderer;

interface IRenderer {
    SlotKey MainEnvironment { get; } // This is stupid. Gotta get scripting working

    void Init();

    void Render();

    void Shutdown();

    SlotKey AddResource(IResource resource);

    void DestroyResource(SlotKey handle);

    IResource? GetResource(SlotKey resource);

    T? GetResourceAs<T>(SlotKey resource) where T : IResource;

    void IncRef(SlotKey resource);

    void DecRef(SlotKey resource);

    SlotKey AddInstance(SlotKey resource, float4x4 transform);

    void DestroyInstance(SlotKey instance);

    ResourceInstance? GetInstance(SlotKey instance);
}
