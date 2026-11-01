using System.Collections.Immutable;
using Brutal.Numerics;

namespace RTS.Engine;

public record struct FlowField(byte[,] Field)
{
    public readonly int2 this[int x, int y] => fieldValueToVector[Field[x, y]];

    // 0 is up, clockwise to 7
    static readonly ImmutableArray<int2> fieldValueToVector = [
        new (0, 1), // n
        new (1, 1), // ne
        new (1, 0),// e
        new (1, -1), // se
        new (0, -1), // s
        new (-1, -1), // sw
        new (-1, 0), // w
        new (-1, 1), // nw
    ];
}
