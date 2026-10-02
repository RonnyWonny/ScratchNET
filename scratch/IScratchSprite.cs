
namespace Scratch.Interface;

public interface IScratchSprite
{
    public void AddBlock(string id, BlockData blockData);
    public void ReadBlock(string id);
    public Blocks GetBlocks();
    public virtual void AddCostume(Costume costume, int index = 0) { }
    public virtual void SetCostume(string CostumeName) { }
    public virtual void SetCostume(int index) { }

}