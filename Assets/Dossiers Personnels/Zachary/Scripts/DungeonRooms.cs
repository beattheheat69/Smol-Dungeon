using UnityEngine;
using UnityEngine.UI;

public class DungeonRooms : MonoBehaviour
{
    [SerializeField] public Vector2 dungeonSize = new Vector2(8, 8);
    public Room[][] rooms;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int x = 0; x < dungeonSize.x; x++)
        {
            for (int y = 0; y < dungeonSize.y; y++)
            {
                rooms[x][y] = new Room();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaceTrap(string entityName, Room room)
    {

    }

}

public class Room
{
    public int id;
    public bool active;
    public string[] enemies;
    public string[] traps;
}