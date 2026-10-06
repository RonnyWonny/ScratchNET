using OpenTK.Mathematics;
using Scratch;
using Scratch.ScratchMath;
using ScratchNET;
using ScratchNET.Common;
using SixLabors.ImageSharp.Drawing.Processing;

public class Sprite(ScratchNet scratch, float x = 0, float y = 0) : Scratch.Sprite(scratch, x, y)
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

    public Texture? texture;
    private Mesh Mesh = new Mesh(Verts, Indices);

    public Costume ThisCostume { get => Costumes[CurrentCostume]; }

    public override void SetCostume(int index)
    {
        base.SetCostume(index);

        var img = ScratchCache.GetImage(ThisCostume.AssetId);
        if (img == null) return;
        texture = Texture.LoadFromImage(img);
    }

    public void Draw()
    {
        if (texture == null || !Visible || Size <= 0) return;
        float xPos = X + WindowProgram.Instance.scratch.CenterX - texture.width / 2;
        float yPos = WindowProgram.Instance.scratch.CenterY - texture.height / 2 - Y;

        float scaleWidth = texture.width * (Size / 100);
        float scaleHeight = texture.height * (Size / 100);

        Mesh.Translate = Matrix4.CreateScale(scaleWidth, scaleHeight, 1f)
            * Matrix4.CreateTranslation(-scaleWidth / 2f, -scaleHeight / 2f, 0f)
            * Matrix4.CreateRotationZ(MathUtil.DegreeToRadian(Direction - 90))
            * Matrix4.CreateTranslation((float)xPos + scaleWidth / 2f, (float)yPos + scaleHeight / 2f, 0f);

        texture.Use();

        Mesh.Draw();
    }
}