using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroParty : MonoBehaviour
{
    public static HeroParty Instance { get; private set; }
    
    List<GameObject> heroList = new List<GameObject>();
    private RoomInstance currentRoom; // RoomInstance the party is currently in
    //To temporarily move party to next rooms
    private bool roomFinished; //If room party is in has monsters or not
    private bool allAtDoor; //If all heros are at the door
    private bool allAtDoor2; //If all heros are at the door
    private bool allAtDoor3; //If all heros are at the door

    void Awake()
    {
        //singleton behaviour
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        currentRoom = GameObject.Find("Room").GetComponent<RoomInstance>();
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
    public RoomInstance GetRoom()
    {
        return currentRoom;
    }

    // Changes if room has monster in or not
    public void SetRoomFinised(bool status)
    {
        roomFinished = status;
    }

    public void RegisterHeroAI(GameObject heroAI)
    {
        if (!heroList.Contains(heroAI))
        {
            heroList.Add(heroAI);
        }
    }

    public void Update()
    {
        if (roomFinished)
        {
            if (currentRoom.name == "Room")
            {
                //move party to door
                GameObject door = GameObject.Find("Door"); // current room door  // toNextRoom[0]
                if (!allAtDoor)
                {
                    allAtDoor = true;
                    // Groups hero in party together
                    foreach (GameObject hero in heroList)
                    {
                        if (Vector2.Distance(hero.transform.position, door.transform.position) > 0.05f)
                        {
                            bool result = hero.transform.GetComponent<HeroAI>().MoveToDoor(door.transform.position);
                            allAtDoor = result && allAtDoor;
                        }
                    }
                }
                if (allAtDoor)
                {
                    allAtDoor2 = true;
                    GameObject door2 = GameObject.Find("Door2"); // current room door  // toNextRoom[1]
                    // Groups hero in party together
                    foreach (GameObject hero in heroList)
                    {
                        if (Vector2.Distance(hero.transform.position, door2.transform.position + new Vector3(0.8f, 0f, 0f)) > 0.05f)
                        {
                            bool result2 = hero.transform.GetComponent<HeroAI>().MoveToDoor(door2.transform.position + new Vector3(0.8f, 0f, 0f));
                            allAtDoor2 = result2 && allAtDoor;
                        }
                    }
                    if (allAtDoor2)
                    {
                        roomFinished = false;
                        currentRoom = GameObject.Find("Room2").GetComponent<RoomInstance>();  // toNextRoom[1].GetParent()
                    }
                }
            }
            else
            {
                //move party to door
                GameObject door3 = GameObject.Find("Door3"); // current room door  // toNextRoom[0]
                if (!allAtDoor3)
                {
                    allAtDoor3 = true;
                    // Groups hero in party together
                    foreach (GameObject hero in heroList)
                    {
                        if (Vector2.Distance(hero.transform.position, door3.transform.position) > 0.05f)
                        {
                            bool result = hero.transform.GetComponent<HeroAI>().MoveToDoor(door3.transform.position);
                            allAtDoor3 = result && allAtDoor3;
                        }
                    }
                }

                // Moves party to next room
                if (allAtDoor3)
                {
                    SceneManager.LoadSceneAsync("BossGym");
                }
            } 
        }
    }
}
