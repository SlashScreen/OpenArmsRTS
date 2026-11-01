namespace RTS.Engine;

class Map(int sizeX, int sizeY) {
    readonly Cell[,] cellMap = new Cell[sizeX, sizeY];

    public Cell this[int x, int y] => cellMap[x, y];

    /* TODO
    - Calculate flow field from group of cells to group of cells
        - Passability
        - Ease of travel (slope, material, layers)
    - Cell chunks
        - Precompute movability between chunk borders
        - Cache flow fields
    - Editable?
    - Precompute passable cells (Maybe making the cost huge)
    */
}
