using System.Numerics;
using Brutal.Numerics;

namespace RTS.Engine.Components;

record struct TransformComponent {
    public float4x4 Xform;

    public float3 Position
    {
        readonly get => Xform.Translation;
        set => Xform.Translation = value;
    }

    // TODO: Set
    public readonly floatQuat Quaternion
    {
        get => floatQuat.CreateFromRotationMatrix(Xform);
    }

    /// Translation, Rotation, Scale
    public readonly (float3, floatQuat, float3) Components
    {
        get
        {
            float4x4.Decompose(Xform, out var scale, out var rot, out var translation);
            return (translation, rot, scale);
        }
    }

    // TODO:
    // Rotation
    // Quaternion

    public TransformComponent() => Xform = float4x4.Identity;
    public TransformComponent(float4x4 xform) => Xform = xform;
}

static class TransformComponentExt {
    public static void Translate(this ref TransformComponent t, float3 delta)
    {
        t.Xform *= float4x4.CreateTranslation(delta);
    }

    public static void Rotate(this ref TransformComponent t, floatQuat quaternion)
    {
        t.Xform *= float4x4.CreateFromQuaternion(quaternion);
    }

    public static void Rotate(this ref TransformComponent t, float3 euler)
    {
        t.Xform *= float4x4.CreateFromYawPitchRoll(euler.X, euler.Y, euler.Z);
    }

    public static void Scale(this ref TransformComponent t, float3 scale) {
        t.Xform *= float4x4.CreateScale(scale);
    }

    public static void Multiply(this ref TransformComponent t, float4x4 other) {
        t.Xform *= other;
    }
}
