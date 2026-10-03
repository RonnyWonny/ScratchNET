using Scratch;
using ScratchNET;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

public class Sprite(float x = 0, float y = 0) : Scratch.Sprite(x, y)
{
    private static readonly float[] Verts = {
        1f,  1f, 0.0f, 1.0f, 1.0f,
        1f, 0f, 0.0f, 1.0f, 0.0f,
        0f, 0f, 0.0f, 0.0f, 0.0f,
        0f,  1f, 0.0f, 0.0f, 1.0f
    };

    private static readonly uint[] Indices = {
        0, 1, 3,
        1, 2, 3
    };

    //private Mesh Mesh = new Mesh(Verts, Indices);

    public Image? texture;

    public Costume ThisCostume { get => Costumes[CurrentCostume]; }

    public override void SetCostume(int index)
    {
        base.SetCostume(index);

        texture = ScratchCache.GetImage(ThisCostume.AssetId);

        //if (image == null) return;
        //texture = Texture.LoadFromImage((Image<Rgba32>)image);
    }

    public void Draw(DrawingCanvas canvas)
    {
        if (texture == null || !Visible) return;
        int xPos = (int)(X + WindowProgram.Instance.scratch.CenterX - texture.Width / 2);
        int yPos = (int)(WindowProgram.Instance.scratch.CenterY - texture.Height / 2 - Y);
        
        canvas.DrawImage(texture, new Rectangle(0, 0, texture.Width, texture.Height), new RectangleF(xPos, yPos, texture.Width, texture.Height), KnownResamplers.NearestNeighbor);
    }
}