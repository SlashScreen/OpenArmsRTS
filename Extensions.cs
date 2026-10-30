using System.Numerics;
using System.Runtime.CompilerServices;
using Brutal.Numerics;

namespace RTS;

public static class RTSExtensions {
    public static Vector3 ToNumerics(this float3 v) => Unsafe.BitCast<float3, Vector3>(v);
    public static float3 ToBrutal(this Vector3 v) => Unsafe.BitCast<Vector3, float3>(v);
    public static Vector4 ToNumerics(this float4 v) => Unsafe.BitCast<float4, Vector4>(v);
    public static Matrix4x4 ToNumerics(this float4x4 m) => Matrix4x4.Create(m.X.ToNumerics(), m.Y.ToNumerics(), m.Z.ToNumerics(), m.W.ToNumerics());

    extension<T>(T src)
    {
        public T Apply(Action<T> fn)
        {
            fn(src);
            return src;
        }

        public T Also(Action fn) {
            fn();
            return src;
        }
    }

    extension<T>(T? src)
    {
        public U? Let<U>(Func<T, U> fn)
        {
            if (src == null)
            {
                return fn(src!);
            }
            return default;
        }

        public void Let(Action<T> fn)
        {
            if (src == null)
            {
                fn(src!);
            }
        }
    }

    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        where TValue : new()
    {
        if (!dict.TryGetValue(key, out TValue val))
        {
            val = new TValue();
            dict.Add(key, val);
        }

        return val;
    }
}
