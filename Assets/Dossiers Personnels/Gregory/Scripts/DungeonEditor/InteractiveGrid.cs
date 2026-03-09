using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveGrid : MonoBehaviour
{
    #region Attributes & Properties
    [field: SerializeField, Header("Grid Size Parameters"), Tooltip("How many rooms the grid is made of on the X axis.")]
    public int GridSizeX {  get; private set; }

    [field: SerializeField, Tooltip("How many rooms the grid is made of on the Y axis.")]
    public int GridSizeY { get; private set; }

    [field: SerializeField, Tooltip("How many tiles a room is made of on the X axis")]
    public float RoomSizeX { get; private set; }

    [field: SerializeField, Tooltip("How many tiles a room is made of on the Y axis")]
    public float RoomSizeY { get; private set; }

    [field: SerializeField, Tooltip("The maximum number of rooms that can be created in the grid. Set to 0 for unlimited.")]
    public int MaxRooms { get; private set; }

    [field: SerializeField, Space, Header("Grid Visual Parameters"), Tooltip("The texture that the line renderer will use to render base gridlines.")]
    public Sprite GridlineBaseTexture { get; private set; }

    [field: SerializeField, Tooltip("The texture that the line renderer will use to render selected cells gridlines.")]
    public Sprite SelectedCellLineTexture { get; private set; }

    [field: SerializeField, Tooltip("The texture the line renderer will use to render hovered cell gridlines.")]
    public Sprite HoveredCellLineTexture { get; private set; }

    [field: SerializeField, Space, Header("Dungeon Validation & Pathfinding Visual Parameters"), Tooltip("Color used to indicate entrance room and boss room")]
    public Color SpecialRoomsColor { get; private set; } = Color.black;

    [field: SerializeField, Tooltip("Color used to indicate that no valid path from entrance to boss room was found")]
    public Color NoValidPathColor { get; private set; } = Color.red;

    [field: SerializeField, Tooltip("Color used to indicate that a valid path from entrance to boss room was found")]
    public Color ValidPathColor { get; private set; } = Color.green;

    [field: SerializeField, Tooltip("Room gamobject for debug purposes. Will be used to visualize the rooms in the grid.")]
    public GameObject RoomPrefab { get; private set; }


    public static Action<string> OnGridEventLogAdded;

    public static GameObject RoomDebugPrefab { get; private set; }

    private static InteractiveGrid _instance;
    public static InteractiveGrid Instance { get { return _instance; }  }

    GridCell[,] _dungeonGrid;
    private bool _dungeonShapingMode = true;
    private bool _entrancePlaced = false;
    private GridCell _entranceCell;
    private bool _bossRoomPlaced = false;
    private GridCell _bossRoomCell;
    private bool _isValidDungeon = false;
    private int _currentRoomsCount = 0;
    private Room _currentRoom; // Used for pathfinding algorithm visualization only
    #endregion

    #region MonoBehaviour Flow
    private void Awake()
    {
        if (_instance == null) { _instance = this; }
        else if (_instance != this){ Destroy(gameObject); }

        RoomDebugPrefab = RoomPrefab;
    }
    #endregion

    #region Grid and Gridcells Management Methods
    public void CreateGrid()
    {
        _dungeonGrid = new GridCell[GridSizeX, GridSizeY];

        for(int i = 0; i < GridSizeX; i++)
        {
            for(int j = 0; j < GridSizeY; j++)
            {
                Vector2 cellWorldPos = CellWorldPos(i, j);
                _dungeonGrid[i, j] = new GridCell(i, j, cellWorldPos, new Vector2(RoomSizeX, RoomSizeY));
            }
        }
        OnGridEventLogAdded?.Invoke($"Created a new grid with size {GridSizeX}x{GridSizeY} and room size {RoomSizeX}x{RoomSizeY}");
        PlaceEntrance();
    }

    private Vector2 CellWorldPos(int cellGridPosX, int cellGridPosY)
    {
        return new Vector2(cellGridPosX * RoomSizeX + RoomSizeX / 2, cellGridPosY * RoomSizeY + RoomSizeY / 2);
    }

    public GridCell GetCellAtGridPos(int gridPosX, int gridPosY)
    {
        if (gridPosX < 0 || gridPosY < 0) return null;
        if (gridPosX > GridSizeX - 1 || gridPosY > GridSizeY - 1) return null;
        return _dungeonGrid[gridPosX, gridPosY];
    }

    public GridCell GetCellAtWorldPos(Vector2 worldPos)
    {
        int gridX = Mathf.FloorToInt(worldPos.x / RoomSizeX);
        int gridY = Mathf.FloorToInt(worldPos.y / RoomSizeY);
        if (gridX < 0 || gridY < 0) return null;
        if (gridX > GridSizeX - 1 || gridY > GridSizeY - 1) return null;
        return _dungeonGrid[gridX, gridY];
    }

    private List<GridCell> GetNeighbouringCells(GridCell cell)
    {
        List<GridCell> neighbours = new List<GridCell>();
        GridCell neighbour;

        neighbour = GetCellAtGridPos(cell.GridPosX + 1, cell.GridPosY); // Right neighbour
        if (neighbour != null) neighbours.Add(neighbour);

        neighbour = GetCellAtGridPos(cell.GridPosX - 1, cell.GridPosY); // Left neighbour
        if (neighbour != null) neighbours.Add(neighbour);

        neighbour = GetCellAtGridPos(cell.GridPosX, cell.GridPosY + 1); // Top neighbour
        if (neighbour != null) neighbours.Add(neighbour);

        neighbour = GetCellAtGridPos(cell.GridPosX, cell.GridPosY - 1); // Bottom neighbour
        if (neighbour != null) neighbours.Add(neighbour);

        return neighbours;
    }

    private void TrySelectCell(Vector2 mouseScreenPos)
    {
        if (!_dungeonShapingMode) return;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        GridCell clickedCell = GetCellAtWorldPos(mouseWorldPos);
        if (clickedCell == null) return;
        
        if (!clickedCell.IsRoom)
        {
            if (_currentRoomsCount >= MaxRooms && MaxRooms != 0)
            {
                OnGridEventLogAdded?.Invoke("Cannot create room: maximum number of rooms reached.");
                return;
            }
            clickedCell.CreateRoom();
            ConnectNeighbouringRooms(clickedCell);

            if (!_bossRoomPlaced)
            {
                clickedCell.RoomAtCell.isBossRoom = true;
                clickedCell.RoomAtCell.DebugSetColor(SpecialRoomsColor);
                _bossRoomCell = clickedCell;
                _bossRoomPlaced = true;
                OnGridEventLogAdded?.Invoke($"Placed boss room at grid cell ({clickedCell.GridPosX}, {clickedCell.GridPosY})");
                return;
            }
            _currentRoomsCount++;
            return;
        }

        if (clickedCell.RoomAtCell.isEntrance)
        {
            OnGridEventLogAdded?.Invoke("Cannot remove Entrance.");
            return;
        }
        if (clickedCell.RoomAtCell.isBossRoom)
        {
            clickedCell.RoomAtCell.isBossRoom = false;
            _bossRoomCell = null;
            _bossRoomPlaced = false;
            OnGridEventLogAdded?.Invoke($"Removed boss room from grid cell ({clickedCell.GridPosX}, {clickedCell.GridPosY})");
        }
        else
        {
            _currentRoomsCount--;
        }
        DisconnectNeighbouringRooms(clickedCell);
        clickedCell.RemoveRoom();
    }

    public void ResetGrid()
    {
        for (int i = 0; i < GridSizeX; i++)
        {
            for (int j = 0; j < GridSizeY; j++)
            {
                if (_dungeonGrid[i, j].IsRoom)
                {
                    DisconnectNeighbouringRooms(_dungeonGrid[i, j]);
                    _dungeonGrid[i, j].RemoveRoom();
                }
            }
        }
        _entrancePlaced = false;
        _bossRoomPlaced = false;
        _isValidDungeon = false;
        _currentRoomsCount = 0;
        _currentRoom = null; // Reset the current room used for pathfinding algorithm visualization
        ExplorationGraph.ResetExploration();
        OnGridEventLogAdded?.Invoke("Dungeon grid reset.");
    }
    #endregion

    #region Room Management Methods
    private void ConnectNeighbouringRooms(GridCell cell)
    {
        foreach (GridCell neighbourCell in GetNeighbouringCells(cell))
        {
            if (neighbourCell.IsRoom)
            {
                cell.RoomAtCell.ConnectRoom(neighbourCell.RoomAtCell); // Connect the newly created room to its neighbour
                neighbourCell.RoomAtCell.ConnectRoom(cell.RoomAtCell); // Connect the neighbour to the newly created room
            }
        }
    }

    private void DisconnectNeighbouringRooms(GridCell cell)
    {
        foreach (GridCell neighbourCell in GetNeighbouringCells(cell))
        {
            if (neighbourCell.IsRoom)
            {
                neighbourCell.RoomAtCell.DisconnectRoom(cell.RoomAtCell); // Disconnect the neighbour from the room that is about to be removed
            }
        }
    }
    #endregion

    #region Dungeon Utility Methods
    public void ToggleDungeonShapingMode()
    {
        _dungeonShapingMode = !_dungeonShapingMode;
    }

    private void PlaceEntrance()
    {
        int gridPosX = 0;
        int gridPosY = 0;
        char[] possibleEdge = { 'T', 'L', 'B', 'R' };
        int randomIndex = UnityEngine.Random.Range(0, possibleEdge.Length);

        switch (possibleEdge[randomIndex])
        {
            case 'T':
                gridPosX = UnityEngine.Random.Range(0, GridSizeX);
                gridPosY = GridSizeY - 1;
                break;

            case 'L':
                gridPosX = 0;
                gridPosY = UnityEngine.Random.Range(0, GridSizeY);
                break;

            case 'B':
                gridPosX = UnityEngine.Random.Range(0, GridSizeX);
                gridPosY = 0;
                break;

            case 'R':
                gridPosX = GridSizeX - 1;
                gridPosY = UnityEngine.Random.Range(0, GridSizeY);
                break;
        }

        _entranceCell = GetCellAtGridPos(gridPosX, gridPosY);
        _entranceCell.CreateRoom();
        _entranceCell.RoomAtCell.isEntrance = true;
        _entranceCell.RoomAtCell.DebugSetColor(SpecialRoomsColor);
        _entrancePlaced = true;
        _currentRoom = _entranceCell.RoomAtCell; // Set the current room to the entrance for pathfinding algorithm visualization
        OnGridEventLogAdded?.Invoke($"Placed entrance at grid cell ({gridPosX}, {gridPosY})");
    }

    private void SetCurrentRoom()
    {
        if (_isValidDungeon)
        {
            _currentRoom = ExplorationGraph.NextRoomToExplore(_currentRoom);
            _currentRoom.DebugSetColor(Color.blue);
            OnGridEventLogAdded?.Invoke($"Exploring next room: ({_currentRoom.GridPosX}, {_currentRoom.GridPosY})");
            return;
        }
        OnGridEventLogAdded?.Invoke("Cannot explore dungeon: no valid path from entrance to boss room.");
    }

    public void ValidateDungeon()
    {
        if (!_entrancePlaced || !_bossRoomPlaced)
        {
            _isValidDungeon = false;
            return;
        }
        HashSet<Room> exploredRooms;
        _isValidDungeon = ExplorationGraph.validatePathToBossRoom(_entranceCell.RoomAtCell, _bossRoomCell.RoomAtCell, out exploredRooms);
        foreach (Room room in exploredRooms)
        {
            if (room.isEntrance || room.isBossRoom) continue;
            room.DebugSetColor(_isValidDungeon ? ValidPathColor : NoValidPathColor);
        }
    }
    #endregion

    #region Enable & Disable
    private void OnEnable()
    {
        Subscribe(true);
    }

    private void OnDisable()
    {
        Subscribe(false);
    }

    private void OnDestroy()
    {
        Subscribe(false);
        if (_instance == this) { _instance = null; }
    }

    private void Subscribe(bool toogleSubscribe)
    {
        if (toogleSubscribe)
        {
            MouseInputsHandler.LeftClick += TrySelectCell;
            IMGUIHelper.OnGenGridButtonPressed += CreateGrid;
            IMGUIHelper.OnResetGridButtonPressed += ResetGrid;
            IMGUIHelper.OnExploreDungeonButtonPressed += SetCurrentRoom;
            IMGUIHelper.OnValidateDungeonButtonPressed += ValidateDungeon;
        }
        else
        {
            MouseInputsHandler.LeftClick -= TrySelectCell;
            IMGUIHelper.OnGenGridButtonPressed -= CreateGrid;
            IMGUIHelper.OnResetGridButtonPressed -= ResetGrid;
            IMGUIHelper.OnExploreDungeonButtonPressed -= SetCurrentRoom;
            IMGUIHelper.OnValidateDungeonButtonPressed -= ValidateDungeon;
        }
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (_dungeonGrid == null) return;
        for (int i = 0; i < GridSizeX; i++)
        {
            for (int j = 0; j < GridSizeY; j++)
            {
                Gizmos.DrawWireCube(_dungeonGrid[i, j].WorldPos, new Vector3(RoomSizeX, RoomSizeY, 0));
            }
        }
    }
    #endregion
}
