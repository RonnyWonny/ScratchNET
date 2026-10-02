
using Scratch.Events;

namespace Scratch.ScratchEvents;

public class EventsGroup : BlocksGroup
{
    public EventsGroup(BlockManager blockManager) : base(blockManager) { }

    public override Dictionary<string, Action<BlockCallEvent>> GetPrimitives()
    {
        return new Dictionary<string, Action<BlockCallEvent>>() {
            {"event_whenflagclicked", WhenFlagClicked }
        };
    }

    private void WhenFlagClicked(BlockCallEvent e)
    {
        Console.WriteLine("CALLED FLAG CLICKED");
        if (e.Sprite == null || e.Next == null) return;
        CallBlock(e.Sprite, e.Next);
    }
}