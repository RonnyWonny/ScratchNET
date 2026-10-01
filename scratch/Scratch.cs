using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Svg;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scratch;

public class ProjectJson
{
    [JsonPropertyName("targets")] public List<ScratchTarget> Targets { get; set; } = [];
}

public class ScratchTarget
{
    public bool isStage { get; set; } = false;
    public string name { get; set; } = "";
    public List<CostumeJson> costumes { get; set; } = [];
    public int currentCostume { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public double size { get; set; }
    public double direction { get; set; } = 90;
    public bool visible { get; set; } = true;
    public int layerOrder { get; set; }
    public string rotationStyle { get; set; } = "";
    public JsonElement blocks { get; set; }
}

public class CostumeJson
{
    public string name { get; set; } = "";
    public string md5ext { get; set; } = "";
    public string dataFormat { get; set; } = "png";
    public double bitmapResolution { get; set; }
    public double rotationCenterX { get; set; }
    public double rotationCenterY { get; set; }
}

public enum ErrorType
{
    OK = 0,
    ProjectFailed = 1,
    AssetFailed = 2,
    SpriteFailed = 3

}

public struct ProjectSettings
{
    public int Width { get; } = 480;
    public int height { get; } = 360;
    public double Framerate { get; } = 30;
    public long BackgroundColor { get; } = 0xFFFFFF;
    public float Scale = 1f;

    public ProjectSettings() { }
}

public class Scratch
{
    public Stage? Stage { get; private set; } = null;
    public List<Sprite> Sprites = [];

    public ProjectSettings Settings = new ProjectSettings();

    private List<object> Assets = [];

    public float CenterX {
        get => Settings.Width / 2f;
    }

    public float CenterY
    {
        get => Settings.height / 2f;
    }

    /// <summary>
    /// initialize with a project file
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public ErrorType Open(string path)
    {
        using var zip = ZipFile.OpenRead(path);

        foreach (ZipArchiveEntry e in zip.Entries)
        {
            try
            {
                if (e.Name.EndsWith(".png"))
                {
                    using Stream ImageStream = e.Open();
                    Image image = Image.Load<Rgba32>(ImageStream);
                    image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
                    ScratchCache.CacheImage(e.Name, image);

                } else if (e.Name.EndsWith(".svg"))
                {
                    using Stream SVGStream = e.Open();
                    SvgDocument svgDocument = SvgDocument.Open<SvgDocument>(SVGStream);

                    using (var bitmap = svgDocument.Draw())
                    {
                        if (bitmap == null) continue;

                        MemoryStream ImageStream = new MemoryStream();
                        bitmap.Save(ImageStream, ImageFormat.Png);
                        ImageStream.Position = 0;

                        Image image = Image.Load(ImageStream);
                        ScratchCache.CacheImage(e.Name, image);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FAILED TO CACHE ASSET: {ex}");
                return ErrorType.AssetFailed;
            }
        }

        var entry = zip.GetEntry("project.json");
        
        if (entry == null)
            return ErrorType.ProjectFailed;

        using var stream = entry.Open();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        ProjectJson? project = JsonSerializer.Deserialize<ProjectJson>(stream, options: options);

        if (project == null)
            return ErrorType.ProjectFailed;

        foreach (ScratchTarget target in project.Targets)
            AddTarget(target);

        return ErrorType.OK;
    }

    protected void AddSpriteAssetsByTarget(Sprite sprite, ScratchTarget target)
    {
        foreach (CostumeJson costume in target.costumes)
        {
            Costume newCostume = new Costume
            {
                Name = costume.name,
                AssetId = costume.md5ext,
                RotationCenterX = costume.rotationCenterX,
                RotationCenterY = costume.rotationCenterY,
                DataFormat = costume.dataFormat,
                BitmapResolution = costume.bitmapResolution
            };

            sprite.AddCostume(newCostume);
        }
    }

    public virtual void AddTarget(ScratchTarget target)
    {
        Sprite sprite = new Sprite()
        {
            Name = target.name,
            X = target.x,
            Y = target.y,
            Size = target.size,
            Direction = target.direction,
            LayerOrder = target.layerOrder,
            Visible = target.visible,
            RotationStyle = target.rotationStyle,
        };

        AddSpriteAssetsByTarget(sprite, target);

        sprite.SetCostume(target.currentCostume);
        Sprites.Add(sprite);
    }

    public virtual void Step()
    {
        foreach (Sprite spr in Sprites)
            spr.ReadBlocks();
    }
}
