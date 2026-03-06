using System.Collections.Generic;
using UnityEngine;

public class HeroParty : MonoBehaviour
{
    public static HeroParty Instance { get; private set; }
    private Room currentRoom; // Room the party is currently in
    private List<GameObject> heroList = new List<GameObject>();
    private bool roomFinished; //If room party is in has monsters or not
    private bool allAtDoor; //If all heros are at the door
    private bool atDoor; //If party at the door

    void Awake()
    {
        //singleton behaviour
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        //Fill list with all heros
        foreach (Transform child in this.transform)
        {
            heroList.Add(child.gameObject);
        }
        roomFinished = false;

        //First Room of dungeon
        currentRoom = GameObject.Find("Room1").GetComponent<Room>();
    }

    // Gives list of heros
    public List<GameObject> GetList()
    {
        return heroList;
    }

    // Gives if room as monsters or not
    public bool GetRoomFinised()
    {
        return roomFinished;
    }

    //Gives current room
    public Room GetRoom()
    {
        return currentRoom;
    }

    // Changes if room has monster in or not
    public void SetRoomFinised(bool status)
    {
        roomFinished = status;
    }

    // If room has no monsters, moves heros and party to next room
    private void Update()
    {
        if (roomFinished)
        {
            //move party to door
            GameObject door = GameObject.Find("Door"); // current room door  // toNextRoom[0]
            if (Vector2.Distance(transform.position, door.transform.position) > 0.15f && !atDoor)
            {
                transform.position = Vector2.MoveTowards(transform.position, door.transform.position, 1f * Time.deltaTime);
            }
            else
            {
                atDoor = true;
            }
            allAtDoor = true;
            // Groups hero in party together
            foreach (GameObject hero in heroList)
            {
                if (Vector2.Distance(hero.transform.position, transform.position) > 0.05f)
                {
                    bool result = hero.transform.GetComponent<HeroAI>().MoveToDoor(transform.position);
                    allAtDoor = result && allAtDoor;
                }
            }

            // Moves party to next room
            if (allAtDoor)
            {
                GameObject door2 = GameObject.Find("Door2"); // current room door  // toNextRoom[1]
                if (Vector2.Distance(transform.position, door2.transform.position + new Vector3(1f, 0f, 0f)) > 0.15f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, door2.transform.position + new Vector3(1f, 0f, 0f), 1f * Time.deltaTime);
                }
                else
                {
                    roomFinished = false;
                    currentRoom = GameObject.Find("Room2").GetComponent<Room>();  // toNextRoom[1].GetParent()
                }
            }
        }

    }
}
