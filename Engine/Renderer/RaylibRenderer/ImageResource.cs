using System.Collections.Immutable;
using System.Text;
using FlashyDJ.Slotmaps;
using Raylib_cs;

namespace RTS.Engine.Renderer.RaylibRenderer;

class ImageResource(Image image) : IResource
{
    public Image Image => image;

    public int RefCount { get; private set; }
    public void Reference() => RefCount++;
    public void Dereference() => RefCount--;

    public void Dispose()
    {
        Raylib.UnloadImage(image);
    }

    public static ImageResource LoadFromMemory(byte[] buffer, string extension)
    {
        unsafe
        {
            fixed (byte* buffPtr = buffer)
            {
                fixed (byte* ext = Encoding.ASCII.GetBytes(extension))
                {
                    return new(Raylib.LoadImageFromMemory((sbyte*)ext, buffPtr, buffer.Length));
                }
            }
        }
    }
    public static ImageResource LoadFromMemory(byte[] buffer, ImageFormats extension) => LoadFromMemory(buffer, formatStrings[extension]);
    public static ImageResource LoadFromPath(string path)
    {
        if (!Path.HasExtension(path)) throw new Exception("Image must be file.");
        var ext = Path.GetExtension(path);
        if (!formatStrings.ContainsValue(ext)) throw new Exception("Image must be supported format.")
        ;
        if (!Engine.Instance.VFS.TryResolvePath(path, out string resolvedPath)) throw new Exception($"Could not find file at path {path}");
        if (Engine.Instance.VFS.TryReadFileResolved(resolvedPath, out byte[] data)) return LoadFromMemory(data, ext);
        else throw new Exception($"Could not read file at path {path}");
    }
    public static ImageResource LoadFromTexture(SlotKey texture) {
        var ri = Engine.Instance.Renderer ?? throw new Exception("No renderer.");
        return new(Raylib.LoadImageFromTexture(ri.GetResourceAs<Texture2DResource>(texture)?.Texture ?? throw new InvalidRidTypeException<Texture2DResource>()));
    }

    public enum ImageFormats
    {
        PNG,
        BMP,
        TGA,
        JPG,
        GIF,
        QOI,
        PSD,
        DDS,
        HDR,
        KTX,
        ASTC,
        PKM,
        PVR,
    }

    static readonly ImmutableDictionary<ImageFormats, string> formatStrings = ImmutableDictionary.CreateRange([
        KeyValuePair.Create(ImageFormats.PNG, ".png"),
        KeyValuePair.Create(ImageFormats.BMP, ".bmp"),
        KeyValuePair.Create(ImageFormats.TGA, ".tga"),
        KeyValuePair.Create(ImageFormats.JPG, ".jpg"),
        KeyValuePair.Create(ImageFormats.GIF, ".gif"),
        KeyValuePair.Create(ImageFormats.QOI, ".qoi"),
        KeyValuePair.Create(ImageFormats.PSD, ".psd"),
        KeyValuePair.Create(ImageFormats.DDS, ".dds"),
        KeyValuePair.Create(ImageFormats.HDR, ".hdr"),
        KeyValuePair.Create(ImageFormats.KTX, ".ktx"),
        KeyValuePair.Create(ImageFormats.ASTC, ".astc"),
        KeyValuePair.Create(ImageFormats.PKM, ".pkm"),
        KeyValuePair.Create(ImageFormats.PVR, ".pvr"),
    ]);
}
