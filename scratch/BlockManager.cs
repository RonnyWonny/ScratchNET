using Scratch.Events;
using Scratch.Interface;
using Scratch.ScratchEvents;

namespace Scratch;

public class BlockManager
{
    public Dictionary<string, Dictionary<string, Action<BlockCallEvent>>> BlockTypes;

    public Scratch Scratch;

    public BlockManager(Scratch scratch)
    {
        this.Scratch = scratch;

        BlockTypes = new Dictionary<string, Dictionary<string, Action<BlockCallEvent>>>()
        {
            {"event", new EventsGroup(this).GetPrimitives()},
            {"motion", new MotionGroup(this).GetPrimitives()}

        };
    }


    public void CallblockFromSprite(IScratchSprite sprite, BlockData block)
    {
        if (block.opcode == null)
        {
            Console.Error.WriteLine($"block opcode is null.");
            return;
        }

        string Group = block.opcode.Split("_")[0];

        if (!BlockTypes.TryGetValue(Group, out Dictionary<string, Action<BlockCallEvent>>? blocksGroup)) return;
        if (!blocksGroup.TryGetValue(block.opcode, out Action<BlockCallEvent>? blockFunction)) return;

        BlockCallEvent @event = new BlockCallEvent(block, sprite)
        {
            Next = block.next,
            Parent = block.parent,
            Parameters = block.inputs
        };

        blockFunction.Invoke(@event);

        if (block.next != null)
            CallblockFromSprite(sprite, block.next);
    }
}