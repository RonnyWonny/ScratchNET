using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Scratch;
using Scratch.Interface;
using ScratchNET.Common;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ScratchNET;

public class ScratchNet : Scratch.Scratch
{
    private Mesh Mesh;
    private Texture Texture;

    public Image<Rgba32> Image;


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

    public ScratchNet() : base()
    {
        Image = new Image<Rgba32>(Settings.Width, Settings.height);
        Texture = new Texture() {
            width = Image.Width,
            height = Image.Height
        };

        //Texture.Use();
        //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Image.Width, Image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);


        Mesh = new Mesh(Verts, Indices);
        Mesh.Translate = Matrix4.CreateScale(Image.Width, Image.Height, 1f);
    }

    public override void AddTarget(ScratchTarget target)
    {
        Sprite sprite = new Sprite(target.x, target.y)
        {
            Name = target.name,
            Size = target.size,
            Direction = target.direction,
            LayerOrder = target.layerOrder,
            Visible = target.visible,
            RotationStyle = target.rotationStyle
        };

        SetupSprite(sprite, target);
        Sprites.Add(sprite);
    }

    public void Draw()
    {
        Image.Mutate(ctx => ctx.Paint(canvas =>
        {
            canvas.Clear(Brushes.Solid(Color.White));
            foreach (Sprite spr in Sprites)
                spr.Draw(canvas);
        }));
        
        var pixels = new byte[Image.Width * Image.Height * 4];
        Image.CopyPixelDataTo(pixels);

        Texture.Use();
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Image.Width, Image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
        Texture.SetPramas();

        Mesh.Draw();
    }

    public void WindowResize(int width, int height)
    {
        float targetAspect = (float)Image.Width / Image.Height;
        float windowAspect = (float)width / height;

        int ViewportWidth;
        int ViewportHeight;

        if (windowAspect > targetAspect)
        {
            ViewportHeight = height;
            ViewportWidth = (int)(height * targetAspect);
        } else
        {
            ViewportWidth = width;
            ViewportHeight = (int)(width / targetAspect);
        }

        float ViewportX = (width - ViewportWidth) / 2f;
        float ViewportY = (height - ViewportHeight) / 2f;

        Mesh.Translate = Matrix4.CreateScale(ViewportWidth, ViewportHeight, 1f) * Matrix4.CreateTranslation(ViewportX, ViewportY, 0f);
    }
}