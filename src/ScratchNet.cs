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
    public override void AddTarget(ScratchTarget target)
    {
        Sprite sprite = new Sprite(this, target.x, target.y)
        {
            Name = target.name,
            Size = target.size,
            Direction = target.direction,
            LayerOrder = target.layerOrder,
            Visible = target.visible,
            RotationStyle = DirectionStyle.AllDirection
        };

        SetupSprite(sprite, target);
        Sprites.Add(sprite);
    }

    public void Draw()
    {
        foreach (Sprite sprite in Sprites)
            sprite.Draw();
    }

    public void WindowResize(int width, int height)
    {
        // to do..
    }
}