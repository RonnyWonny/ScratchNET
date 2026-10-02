using Scratch;
using Scratch.Events;

public class MotionGroup : BlocksGroup
{
    public MotionGroup(BlockManager blockManager) : base(blockManager) { }

    public override Dictionary<string, Action<BlockCallEvent>> GetPrimitives()
    {
        return new Dictionary<string, Action<BlockCallEvent>>() {
            {"motion_turnright", TurnRight},
            {"motion_movesteps", MoveSteps}
        };
    }

    private void TurnRight(BlockCallEvent e)
    {
        Console.WriteLine("TURNING SPRITE!!");
        if (e.Sprite is Sprite spr)
        {
            spr.Direction += (float)e.Parameters["DEGREES"];
            CallBlock(e.Sprite, e.Next);
        }
    }

    private void MoveSteps(BlockCallEvent e)
    {
        Console.WriteLine("MOVING SPRITE!!");
        if (e.Sprite is Sprite spr)
        {
            spr.X += (float)Math.Sin(ScratchMath.DegreeToRadian(spr.Direction)) * (float)e.Parameters["STEPS"];
            spr.Y += (float)Math.Cos(ScratchMath.DegreeToRadian(spr.Direction)) * (float)e.Parameters["STEPS"];
            CallBlock(e.Sprite, e.Next);
        }
    }
}
