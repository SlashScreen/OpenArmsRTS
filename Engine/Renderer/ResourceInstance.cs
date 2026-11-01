using Brutal.Numerics;
using FlashyDJ.Slotmaps;

namespace RTS.Engine.Renderer;

public class ResourceInstance: IDisposable {
    public SlotKey RID {
        get => rid;
        set
        {
            Engine.Instance.Renderer?.DecRef(rid);
            Engine.Instance.Renderer?.IncRef(value);
            rid = value;
        }
    }
    public float4x4 Transform { get; set; }

    private SlotKey rid;

    public ResourceInstance(SlotKey rid, float4x4 transform) {
        this.rid = rid;
        Transform = transform;
        Engine.Instance.Renderer?.IncRef(rid);
    }

    public void Dispose()
    {
        Engine.Instance.Renderer?.DecRef(rid);
    }
}
