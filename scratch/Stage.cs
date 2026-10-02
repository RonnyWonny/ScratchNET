using Scratch.Interface;

namespace Scratch;

public partial class Stage : IScratchSprite
{
    public void AddBlock(string id, BlockData blockData)
    {
        throw new NotImplementedException();
    }

    public void AddCostume(Costume costume, int index = 0)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, BlockData> GetBlocks()
    {
        throw new NotImplementedException();
    }

    public void ReadBlock(string id)
    {
        throw new NotImplementedException();
    }

    Blocks IScratchSprite.GetBlocks()
    {
        throw new NotImplementedException();
    }
}
