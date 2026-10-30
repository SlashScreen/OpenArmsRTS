using Brutal.Numerics;
using FlashyDJ.Slotmaps;

namespace RTS.Engine.Renderer;

interface IRenderer {
    void Init();

    void Render();

    void Shutdown();

    SlotKey AddResource(IResource resource);

    void DestroyResource(SlotKey handle);

    IResource? GetResource(SlotKey resource);

    T? GetResourceAs<T>(SlotKey resource) where T: IResource;

    SlotKey AddInstance(SlotKey resource, float4x4 transform);

    void DestroyInstance(SlotKey instance);

    ResourceInstance? GetInstance(SlotKey instance);
}
