using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ScratchNET.Common;

public unsafe class Texture
{
    int Handle;

    public int width;
    public int height;

    public static Texture LoadFromImage(Image<Rgba32> image)
    {
        byte[] pixels = new byte[image.Width * image.Height * 4];
        image.CopyPixelDataTo(pixels);

        var texture = new Texture()
        {
            width = image.Width,
            height = image.Height
        };
        texture.Use();

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        texture.SetPramas();

        return texture;
    }

    public Texture()
    {
        Handle = GL.GenTexture();
        SetPramas();
    }

    private void SetPramas()
    {
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }

    public void Use(TextureUnit unit)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    public void Use()
    {
        Use(TextureUnit.Texture0);
    }

    public void SetData(System.Drawing.Rectangle bounds, byte[] data)
    {
        Use();
        fixed (byte* ptr = data)
        {
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, bounds.Left, bounds.Top, bounds.Right, bounds.Bottom, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        }
    }
}