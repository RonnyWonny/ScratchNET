using Scratch;
using Scratch.Events;

public class MotionGroup : BlocksGroup
{
    public MotionGroup(BlockManager blockManager) : base(blockManager) { }

    public override Dictionary<string, Action<BlockCallEvent>> GetPrimitives()
    {
        return new Dictionary<string, Action<BlockCallEvent>>() {
            {"motion_movesteps", MoveSteps},
            {"motion_turnright", TurnRight},
            {"motion_turnleft", TurnLeft},
            {"motion_goto", Goto}
        };
    }

    private void MoveSteps(BlockCallEvent e)
    {
        if (e.Sprite is Sprite spr)
        {
            spr.X += (float)Math.Sin(ScratchMath.DegreeToRadian(spr.Direction)) * (float)e.Parameters["STEPS"];
            spr.Y += (float)Math.Cos(ScratchMath.DegreeToRadian(spr.Direction)) * (float)e.Parameters["STEPS"];
        }
    }

    private void TurnRight(BlockCallEvent e)
    {
        if (e.Sprite is Sprite spr)
            spr.Direction += (float)e.Parameters["DEGREES"];
    }

    private void TurnLeft(BlockCallEvent e)
    {
        if (e.Sprite is Sprite spr)
            spr.Direction -= (float)e.Parameters["DEGREES"];
    }

    private void Goto(BlockCallEvent e)
    {
        if (e.Sprite is Sprite spr)
        {
            BlockData TO = (BlockData)e.Parameters["TO"];
            
            switch (((List<object>)TO.fields["TO"])[0]) {
                case "_random_":
                    Random random = new Random();
                    spr.X = random.NextInt64(-BlockManager.Scratch.Settings.Width, BlockManager.Scratch.Settings.Width);
                    spr.Y = random.NextInt64(-BlockManager.Scratch.Settings.height, BlockManager.Scratch.Settings.height);
                    break;
            }
        }
    }
}
