using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Scratch;
using ScratchNET;
using ScratchNET.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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

    private Mesh Mesh = new Mesh(Verts, Indices);

    public Texture? texture;

    public Costume ThisCostume { get => Costumes[CurrentCostume]; }

    public override void SetCostume(int index)
    {
        base.SetCostume(index);

        var image = ScratchCache.GetImage(ThisCostume.AssetId);

        if (image == null) return;
        texture = Texture.LoadFromImage((Image<Rgba32>)image);
    }

    public void Draw()
    {
        if (texture == null || !Visible) return;

        float xPos = X + (float)WindowProgram.Instance.scratch.CenterX - texture.width / 2f;
        float yPos = (float)WindowProgram.Instance.scratch.CenterY - texture.height / 2f - Y;

        Mesh.Translate = Matrix4.CreateScale(texture.width, texture.height, 1f) * Matrix4.CreateTranslation((float)xPos, (float)yPos, 0f);
        texture.Use(TextureUnit.Texture0);

        Mesh.Draw();
    }
}