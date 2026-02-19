public enum GridCellState
{
    Empty,  // Can place a tower
    NotPlaceable,   // Cannot place a tower, there is an obstacle blocking the cell
    TowerOccupied   // Cannot place a tower, there is a tower using the cell
}
