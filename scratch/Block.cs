using Scratch.Events;
using Scratch.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scratch;

public class BlocksGroup(BlockManager blockManager)
{
    protected readonly BlockManager BlockManager = blockManager;

    public virtual Dictionary<string, Action<BlockCallEvent>> GetPrimitives()
    {
        return [];
    }


    protected void CallBlock(IScratchSprite sprite, BlockData? block)
    {
        if (block == null) return;
        BlockManager.CallblockFromSprite(sprite, block);
    }
}