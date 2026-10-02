namespace Scratch;

public class Blocks
{
    public Dictionary<string, BlockData> blocks = [];

    public Dictionary<string, BlockData> FilterBlocksByGroup(string group)
    {
        return blocks.Where(x => x.Value.opcode?.Split("_")[0] == group).ToDictionary();
    }

    public BlockData? GetBlock(string id)
    {
        return blocks.ContainsKey(id) ? blocks[id] : null;
    }
}