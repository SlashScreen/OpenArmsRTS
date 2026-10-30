using Brutal.Numerics;
using FlashyDJ.Slotmaps;

namespace RTS.Engine.Renderer;

public class ResourceInstance(SlotKey rid, float4x4 transform) {
    public SlotKey RID => rid;
    public float4x4 Transform {
        get => transform;
        set => transform = value;
    }
}
