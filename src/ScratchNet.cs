using Scratch;

namespace ScratchNET;

public class ScratchNet : Scratch.Scratch
{
    public override void AddTarget(ScratchTarget target)
    {
        Sprite sprite = new Sprite(target.x, target.y);
        sprite.Name = target.name;
        sprite.Size = target.size;
        sprite.Direction = target.direction;
        sprite.LayerOrder = target.layerOrder;
        sprite.Visible = target.visible;
        sprite.RotationStyle = target.rotationStyle;

        SetupSprite(sprite, target);
        Sprites.Add(sprite);
    }

    public void Draw()
    {
        foreach (var s in Sprites)
        {
            if (s is Sprite sprite)
                sprite.Draw();
        }
    }
}