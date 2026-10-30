namespace RTS.Engine;

using IDSize = UInt32;
using GenSize = byte;

class GenerationalPool<T>
{
    const int OCCUPIED = -1;

    List<IEntry> items = [];


    private interface IEntry;
    private record struct FreeEntry(int Next): IEntry;
    private record struct OccupiedEntry(GenSize Generation, T Value): IEntry;
}

record struct ID {
    const int INDEX_SIZE = sizeof(IDSize) - sizeof(GenSize);

    const IDSize GENERATION_MASK = 0xFF << INDEX_SIZE;
    const IDSize INDEX_MASK = ~GENERATION_MASK;
    const IDSize MAX_INDEX = IDSize.MaxValue >> sizeof(IDSize) - INDEX_SIZE;

    public readonly GenSize Generation => (GenSize)((data & GENERATION_MASK) >> INDEX_SIZE);
    public readonly IDSize Index => data & INDEX_MASK;

    private readonly IDSize data;

    public ID(byte generation, IDSize index)
    {
        if (index > MAX_INDEX) throw new Exception("Max index reached");

        data = ((IDSize)generation << INDEX_SIZE) | index;
    }
}
