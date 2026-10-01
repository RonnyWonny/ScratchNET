using SixLabors.ImageSharp;

namespace Scratch;

public class ScratchCache
{
    public static Dictionary<string, Image> Images = [];
    public static Dictionary<string, object> Sounds = [];

    public static void CacheImage(string id, Image img)
    {
        Images[id] = img;
    }

    public static Image? GetImage(string id)
    {
        if (Images.ContainsKey(id))
            return Images[id];
        return null;
    }
}