using Brutal.Numerics;

namespace RTS.Engine.Renderer;

interface IResource: IDisposable {
    int RefCount { get; }

    void Reference();

    void Dereference();
}
