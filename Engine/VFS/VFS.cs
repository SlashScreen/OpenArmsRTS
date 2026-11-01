using Brutal.Strings;
using Brutal.Strings.Extensions;

namespace RTS.Engine.VFS;

class VFS(List<string> loadOrder) {
    const string MODS_PATH = "mods/";
    const string BUILTIN_PATH = "builtin/";
    const int BUFFER_LEN = 128;

    public bool TryResolvePath(string path, out string resolvedPath)
    {
        var builder = new StringBuilder<byte>(BUFFER_LEN);

        foreach (var modPath in loadOrder)
        {
            builder.Clear();
            builder.Append(MODS_PATH);
            builder.Append(modPath);
            builder.Append("/");
            builder.Append(path);

            if (Utf8.Path.Exists(builder.WrittenSpan))
            {
                resolvedPath = builder.WrittenSpan.ToString16();
                return true;
            }
        }

        // Builtin fallback
        builder.Clear();
        builder.Append(BUILTIN_PATH);
        builder.Append(path);

        if (Utf8.Path.Exists(builder.WrittenSpan))
        {
            resolvedPath = builder.WrittenSpan.ToString16();
            return true;
        }

        resolvedPath = "";
        return false;
    }

    // with unresolved path
    public bool TryReadFile(string path, out string data)
    {
        if (TryResolvePath(path, out var resolvedPath))
        {
            return TryReadFileResolved(resolvedPath, out data);
        }

        data = "";
        return false;
    }

    public bool TryReadFile(string path, out byte[] data)
    {
        if (TryResolvePath(path, out var resolvedPath))
        {
            return TryReadFileResolved(resolvedPath, out data);
        }

        data = [];
        return false;
    }

    // with resolved path
    public bool TryReadFileResolved(string resolvedPath, out string data)
    {
        if (!Path.HasExtension(resolvedPath))
        {
            data = "";
            return false;
        }

        data = File.ReadAllText(resolvedPath);
        return true;
    }

    public bool TryReadFileResolved(string resolvedPath, out byte[] data) {
        if (!Path.HasExtension(resolvedPath))
        {
            data = [];
            return false;
        }

        data = File.ReadAllBytes(resolvedPath);
        return true;
    }
}
