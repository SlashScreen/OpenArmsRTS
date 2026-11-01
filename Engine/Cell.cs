namespace RTS.Engine;

public record struct Cell(int Height, ushort Layers, byte MoveCost);
