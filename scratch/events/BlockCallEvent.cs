using Scratch.Interface;
using System;

namespace Scratch.Events;

public struct BlockCallEvent(BlockData Block, IScratchSprite sprite)
{
    public IScratchSprite Sprite { get; set; } = sprite;

    public bool IsStage { get; set; } = sprite is Stage;

    public BlockData Block { get; set; } = Block;
    public BlockData? Parent { get; set; } = Block.parent;
    public BlockData? Next { get; set; } = Block.next;
    public Dictionary<string, object> Parameters { get; set; } = [];
}