using System;
using System.Collections.Generic;
using System.Text;

namespace Scratch;

public enum BlockType
{
    Hat=0,
    Block=1,
    Operator=2,
    C=3
}

public class Block
{
    public BlockType Type;

    public string Opcode = string.Empty;

    public Block? Parent;
    public Block? Next;

    public bool ScreenRefresh = true;

    public List<object> Inputs = [];

    public virtual object? Call(Dictionary<string, object> args)
    {
        return null;
    }
}