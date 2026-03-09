using UnityEngine;

public class GridCell
{
    public int GridPosX { get; private set; }
    public int GridPosY { get; private set; }
    public Vector2 WorldPos { get; private set; }
    public Vector2 CellSize { get; private set; }
    public Room RoomAtCell {  get; private set; }

    public bool IsRoom { get; set; }

    public GridCell(int gridPosX, int gridPosY, Vector2 worldPos, Vector2 cellSize)
    {
        GridPosX = gridPosX;
        GridPosY = gridPosY;
        WorldPos = worldPos;
        CellSize = cellSize;
    }

    public void CreateRoom()
    {
        if (RoomAtCell == null)
        {
            RoomAtCell = new Room(GridPosX, GridPosY, WorldPos, CellSize);
            IsRoom = true;
            Debug.Log($"Created room at grid cell ({GridPosX}, {GridPosY})");
        }
    }

    public void RemoveRoom()
    {
        if (RoomAtCell != null)
        {
            RoomAtCell.OnRemove();
            RoomAtCell = null;
            IsRoom = false;
            Debug.Log($"Removed room from grid cell ({GridPosX}, {GridPosY})");
        }
    }

    public void PrintInfo()
    {
        Debug.Log($"Grid Cell at ({GridPosX}, {GridPosY}) with world position {WorldPos} and cell size {CellSize}");
    }
}
