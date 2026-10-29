namespace RTS;

public static class RTSExtensions {
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

    extension<T>(T? src) {
        public U? Let<U>(Func<T, U> fn)
        {
            if (src == null)
            {
                return fn(src!);
            }
            return default;
        }
    }
}
