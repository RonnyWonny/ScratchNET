using Scratch.Interface;
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
    public float size { get; set; }
    public float direction { get; set; } = 90;
    public bool visible { get; set; } = true;
    public int layerOrder { get; set; }
    public string rotationStyle { get; set; } = "";
    public Dictionary<string, BlockJson> blocks { get; set; } = [];
}

public class BlockJson
{
    public string? opcode { get; set; }
    public string? next { get; set; }
    public string? parent { get; set; }
    public Dictionary<string, JsonElement> fields { get; set; } = [];
    public Dictionary<string, List<JsonElement>> inputs { get; set; } = [];
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

public class BlockData
{
    public string? opcode = null;
    public string? blockName { get => opcode?.Split("_")[1]; }
    public BlockData? next = null;
    public BlockData? parent = null;
    public Dictionary<string, object> fields = [];
    public Dictionary<string, object> inputs = [];
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
    public BlockManager BlockManager;

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

    public Scratch()
    {
        BlockManager = new BlockManager(this);
    }

    /// <summary>
    /// initialize with a project file
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public ErrorType Open(string path)
    {
        using var zip = ZipFile.OpenRead(path);

        // put this into its own function
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

    protected void AddSpriteAssetsByTarget(IScratchSprite sprite, ScratchTarget target)
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

    protected void AssignBlocksToSprite(IScratchSprite sprite, Dictionary<string, BlockJson> blocks)
    {

        foreach (string id in blocks.Keys)
        {
            BlockJson data = blocks[id];

            Dictionary<string, object> inputs = [];

            Console.WriteLine(data.inputs.Count);
            foreach (string input in data.inputs.Keys)
            {
                var value = data.inputs[input][1][1].ToString();

                Console.WriteLine(value);
                if (float.TryParse(value, out float i))
                    inputs[input] = i;
                else if (bool.TryParse(value, out bool b))
                    inputs[input] = b;
                else if (value != null)
                    inputs[input] = value;
            }

            BlockData block = new BlockData
            {
                opcode = data.opcode,
                inputs = inputs
            };

            sprite.AddBlock(id, block);
        }

        Blocks SpriteBlocks = sprite.GetBlocks();

        foreach (string id in blocks.Keys)
        {
            BlockJson data = blocks[id];

            if (data.next == null) continue;
            BlockData? blockData = SpriteBlocks.GetBlock(id);
            BlockData? nextBlockData = SpriteBlocks.GetBlock(data.next);

            if (blockData == null || nextBlockData == null) continue;

            blockData.next = nextBlockData;
            nextBlockData.parent = blockData;
        }
    }

    protected void SetupSprite(IScratchSprite sprite, ScratchTarget target)
    {
        AddSpriteAssetsByTarget(sprite, target);
        AssignBlocksToSprite(sprite, target.blocks);
        sprite.SetCostume(target.currentCostume);
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

        SetupSprite(sprite, target);
        Sprites.Add(sprite);
    }

    public void Start()
    {
        foreach (IScratchSprite spr in Sprites)
        {
            Blocks blocks = spr.GetBlocks();
            foreach (BlockData block in blocks.FilterBlocksByGroup("event").Values)
            {
                if (block.blockName != "whenflagclicked") continue;
                BlockManager.CallblockFromSprite(spr, block);
            }
        }
    }

    public virtual void Step()
    {
        //foreach (Sprite spr in Sprites)
        //    spr.ReadBlocks();
    }
}
