using Brutal.Numerics;

namespace RTS.Engine.Renderer;

// TODO: Make this a class instead?
interface IResource : IDisposable
{
    int RefCount { get; }

    void Reference();

    void Dereference();
}
