using Microsoft.Extensions.ObjectPool;

namespace RTS.Engine.Renderer;

class ResourceHandle: IResettable {
    public IResource? Resource { get; private set; }

    public ResourceHandle() => Resource = null;

    public void BindResource(IResource resource) => Resource = resource;

    public bool TryReset()
    {
        Resource = null;
        return true;
    }
}
