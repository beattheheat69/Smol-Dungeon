using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int GridPosX {  get; private set; }
    public int GridPosY { get; private set; }
    public Vector2 WorldPos { get; private set; }
    public Vector2 RoomSize { get; private set; }
    public bool isEntrance { get; set; }
    public bool isBossRoom { get; set; }
    public Room ParentRoom { get; set; }
    public List<Room> ConnectedRooms { get; private set; } = new List<Room>();
    private Dictionary<DoorDirection, Vector2> _doors = new Dictionary<DoorDirection, Vector2>();
    private GameObject _roomObject;

    public Room(int gridPosX, int gridPosY, Vector2 worldPos, Vector2 roomSize)
    {
        RoomSize = roomSize;
        GridPosX = gridPosX;
        GridPosY = gridPosY;
        WorldPos = worldPos;
        CreateRoomObject();
    }

    public void ConnectRoom(Room room)
    {
        ConnectedRooms.Add(room);
        CreateDoorToRoom(room);
        Debug.Log($"Connected room at grid pos ({room.GridPosX},{room.GridPosY}) to room at grid pos ({GridPosX},{GridPosY})");
    }

    public void DisconnectRoom(Room room)
    {
        if (ConnectedRooms.Contains(room))
        {
            ConnectedRooms.Remove(room);
            RemoveDoorToRoom(room);
            Debug.Log($"Disconnected room at grid pos ({room.GridPosX},{room.GridPosY}) from room at grid pos ({GridPosX},{GridPosY})");
        }
    }

    private void DisconnectAllRooms()
    {        
        ConnectedRooms.Clear();
        _doors.Clear();
        Debug.Log($"Disconnected all rooms from room at grid pos ({GridPosX},{GridPosY})");
    }

    private void CreateDoorToRoom(Room room)
    {
        if (room.GridPosX > GridPosX)
        {
            _doors.Add(DoorDirection.Right, new Vector2(WorldPos.x + RoomSize.x / 2, WorldPos.y));
            Debug.Log($"Created door to the right of room at grid pos ({GridPosX},{GridPosY})");
        }
        else if (room.GridPosX < GridPosX)
        {
            _doors.Add(DoorDirection.Left, new Vector2(WorldPos.x - RoomSize.x / 2, WorldPos.y));
            Debug.Log($"Created door to the left of room at grid pos ({GridPosX},{GridPosY})");
        }
        else if (room.GridPosY > GridPosY)
        {
            _doors.Add(DoorDirection.Up, new Vector2(WorldPos.x, WorldPos.y + RoomSize.y / 2));
            Debug.Log($"Created door above room at grid pos ({GridPosX},{GridPosY})");
        }
        else if (room.GridPosY < GridPosY)
        {
            _doors.Add(DoorDirection.Down, new Vector2(WorldPos.x, WorldPos.y - RoomSize.y / 2));
            Debug.Log($"Created door below room at grid pos ({GridPosX},{GridPosY})");
        }
    }

    private void RemoveDoorToRoom(Room room)
    {
        if (_doors.Count > 0)
        {
            if (room.GridPosX > GridPosX)
            {
                _doors.Remove(DoorDirection.Right);
                Debug.Log($"Removed door to the right of room at grid pos ({GridPosX},{GridPosY})");
            }
            else if (room.GridPosX < GridPosX)
            {
                _doors.Remove(DoorDirection.Left);
                Debug.Log($"Removed door to the left of room at grid pos ({GridPosX},{GridPosY})");
            }
            else if (room.GridPosY > GridPosY)
            {
                _doors.Remove(DoorDirection.Up);
                Debug.Log($"Removed door above room at grid pos ({GridPosX},{GridPosY})");
            }
            else if (room.GridPosY < GridPosY)
            {
                _doors.Remove(DoorDirection.Down);
                Debug.Log($"Removed door below room at grid pos ({GridPosX},{GridPosY})");
            }
        }
    }

    public Vector2 GetDoorToRoom(Room room)
    {
        if(!ConnectedRooms.Contains(room)) return Vector2.zero; // No door to a room that is not connected

        if (room.GridPosX > GridPosX)
        {
            return _doors[DoorDirection.Right];
        }
        if (room.GridPosX < GridPosX)
        {
            return _doors[DoorDirection.Left];
        }
        if (room.GridPosY > GridPosY)
        {
            return _doors[DoorDirection.Up];
        }
        if (room.GridPosY < GridPosY)
        {
            return _doors[DoorDirection.Down];
        }
        return Vector2.zero; // No door to the specified room
    }

    private void CreateRoomObject()
    {
        _roomObject = Object.Instantiate(InteractiveGrid.RoomDebugPrefab, WorldPos, Quaternion.identity);
    }

    public void DebugSetColor(Color color)
    {
        if (_roomObject != null)
        {
            var renderer = _roomObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = color;
            }
        }
    }

    public override string ToString()
    {
        return $"Room at grid pos ({GridPosX},{GridPosY})  and world pos {WorldPos}. Room size: {RoomSize}";
    }

    public void OnRemove()
    {
        DisconnectAllRooms();        
        Object.Destroy(_roomObject);
    }
}
