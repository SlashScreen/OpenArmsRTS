using Brutal.Numerics;

namespace RTS.Engine.Renderer;

interface IDrawable: IResource {
    void Draw(IRenderer irenderer, IEnumerable<float4x4> transforms);
}
