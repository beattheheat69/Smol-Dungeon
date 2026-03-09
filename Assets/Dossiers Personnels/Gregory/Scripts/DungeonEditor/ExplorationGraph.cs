using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class ExplorationGraph
{
    private static HashSet<Room> _visitedRooms = new HashSet<Room>();

    /// <summary>
    /// Validates whether a path exists from the specified starting room to the boss room using a breadth-first search
    /// algorithm.
    /// </summary>
    /// <remarks>If the start room is the same as the end room, or if either room is null, the method returns
    /// false and sets exploredRooms to null.</remarks>
    /// <param name="startRoom">The room from which the path validation begins. Cannot be null.</param>
    /// <param name="endRoom">The room representing the boss room to which the path is validated. Cannot be null.</param>
    /// <param name="exploredRooms">When this method returns, contains a set of rooms that were explored during the path validation process if a
    /// valid path search was performed; otherwise, null.</param>
    /// <returns>true if a valid path exists from the start room to the boss room; otherwise, false.</returns>
    public static bool validatePathToBossRoom(Room startRoom, Room endRoom, out HashSet<Room> exploredRooms) // BFS. Validates if there is a path from the entrance to the boss room and returns the set of explored rooms
    {
        if (startRoom == endRoom || startRoom == null || endRoom == null)
        {
            exploredRooms = null;
            return false;
        }
        
        HashSet<Room> openSet = new HashSet<Room>();
        HashSet<Room> closedSet = new HashSet<Room>();
        bool isValidPath = false;
        exploredRooms = null;

        openSet.Add(startRoom); // Start exploration from the entrance

        while (openSet.Count > 0)
        {
            Room currentRoom = openSet.First(); // Get an arbitrary room from the open set
            openSet.Remove(currentRoom); // Remove it from the open set
            closedSet.Add(currentRoom); // Mark the current room as explored

            if (currentRoom == endRoom)
            {
                isValidPath = true;
            }
            
            foreach (Room neighbor in currentRoom.ConnectedRooms)
            {
                if (!closedSet.Contains(neighbor))
                {
                    neighbor.ParentRoom = currentRoom; // Set parent for backtracking
                    openSet.Add(neighbor); // Add unvisited neighbors to the open set
                }
            }
        }
        exploredRooms = closedSet; // Return the set of explored rooms
        return isValidPath; // Return whether a valid path to the boss room was found
    }

    /// <summary>
    /// Returns the next unvisited room connected to the specified room for exploration, or backtracks to the parent
    /// room if all connected rooms have been visited.
    /// </summary>
    /// <remarks>This method implements a depth-first search (DFS) traversal strategy for exploring
    /// interconnected rooms. It marks each visited room to prevent revisiting and assigns the parent room to enable
    /// backtracking when necessary. The order of exploration among connected rooms is randomized.</remarks>
    /// <param name="currentRoom">The room from which to continue exploration. This room must not be null and should be part of the exploration
    /// graph.</param>
    /// <returns>The next unvisited connected room to explore, or the parent room if all connected rooms have already been
    /// visited.</returns>
    public static Room NextRoomToExplore(Room currentRoom) // DFS. Returns the next room to explore based on the current room, or backtracks if all connected rooms have been visited
    {
        if (currentRoom == null) return null;
        _visitedRooms.Add(currentRoom); // Mark current room as visited
        Debug.Log($"Exploring from room at ({currentRoom.GridPosX}, {currentRoom.GridPosY})");
        foreach (var connectedRoom in Misc.ShuffleList(currentRoom.ConnectedRooms))
        {
            Debug.Log($"Checking connected room at ({connectedRoom.GridPosX}, {connectedRoom.GridPosY})");
            if (!_visitedRooms.Contains(connectedRoom))
            {
                Debug.Log($"Unvisited room at ({connectedRoom.GridPosX}, {connectedRoom.GridPosY}) !");
                if (currentRoom.ConnectedRooms.Count > 1 ) 
                {
                    Debug.Log($"Setting parent of room at ({connectedRoom.GridPosX}, {connectedRoom.GridPosY}) to room at ({currentRoom.GridPosX}, {currentRoom.GridPosY})");
                    connectedRoom.ParentRoom = currentRoom; // Set parent for backtracking
                }
                Debug.Log($"Moving to room at ({connectedRoom.GridPosX}, {connectedRoom.GridPosY})");
                return connectedRoom; // Move to the next unvisited connected room
            }
        }
        Debug.Log($"All connected rooms from ({currentRoom.GridPosX}, {currentRoom.GridPosY}) have been visited. Backtracking to ({currentRoom.ParentRoom.GridPosX}, {currentRoom.ParentRoom.GridPosY})...");
        return currentRoom.ParentRoom; // Backtrack if all connected rooms have been visited
    }

    /// <summary>
    /// Resets the exploration state by clearing the list of visited rooms, allowing for a new exploration session.
    /// </summary>
    /// <remarks>Call this method before starting a new exploration to ensure that previous session data does
    /// not affect the current exploration. This method is typically used when reinitializing or restarting the
    /// exploration process.</remarks>
    public static void ResetExploration()
    {
        _visitedRooms.Clear(); // Clear the list of visited rooms for a new exploration session
    }

    /// <summary>
    /// Returns the positions of the doors that connect two specified rooms, if a connection exists.
    /// </summary>
    /// <remarks>This method assumes that both rooms are connected. If the rooms are not connected, the
    /// returned positions may not correspond to valid door locations.</remarks>
    /// <param name="roomFrom">The source room from which the connection to the target room is evaluated.</param>
    /// <param name="roomTo">The target room to which the connection is evaluated from the source room.</param>
    /// <returns>An array of two <see cref="Vector2"/> values representing the positions of the doors connecting the two rooms.
    /// The first element is the door position from <paramref name="roomFrom"/> to <paramref name="roomTo"/>, and the
    /// second element is the door position from <paramref name="roomTo"/> to <paramref name="roomFrom"/>.</returns>
    private static Vector2[] ConnectedDoorsPositions(Room roomFrom, Room roomTo)
    {        
        return new Vector2[] { roomFrom.GetDoorToRoom(roomTo), roomTo.GetDoorToRoom(roomFrom) };
    }
}
