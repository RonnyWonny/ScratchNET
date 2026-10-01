namespace Scratch;

public class Blocks
{
    public List<Block> blocks = [];
    public int StepI { get; private set; } = 0;
    public Block CurrentBlock { get =>  blocks[StepI]; }

    public void Step()
    {
        StepI++;
        CurrentBlock.Call([]);
    }
}