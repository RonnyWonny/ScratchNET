using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Scratch;

public class ScratchCache
{
    public static Dictionary<string, Image<Rgba32>> Images = [];
    public static Dictionary<string, object> Sounds = [];

    public static void CacheImage(string id, Image<Rgba32> img)
    {
        Images[id] = img;
    }

    public static Image<Rgba32>? GetImage(string id)
    {
        if (Images.ContainsKey(id))
            return Images[id];
        return null;
    }
}