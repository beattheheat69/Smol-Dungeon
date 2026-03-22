using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroParty : MonoBehaviour
{
    public static HeroParty Instance { get; private set; }
    [SerializeField]
    RoomInstance[] roomList;
    List<GameObject> heroList = new List<GameObject>();
    [SerializeField]
    int currentRoomId; // RoomInstance the party is currently in
    //To temporarily move party to next rooms
    bool roomFinished; //If room party is in has monsters or not
    bool allAtDoorExit; //If all heros are at the door
    bool allAtDoorEntrance; //If all heros are at the door


    void Awake()
    {
        //singleton behaviour
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        currentRoomId = 0;
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
        return roomList[currentRoomId];
    }

    // Changes if room has monster in or not
    public void SetRoomFinised(bool status)
    {
        roomFinished = !roomList[currentRoomId].checkMonsters();
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
            Transform doorExit;
            if (currentRoomId == roomList.Count() - 1)
            {
                doorExit= roomList[currentRoomId].transform.Find("BossDoor");
            }
            else 
            {
                doorExit = roomList[currentRoomId].transform.Find("ExitDoor");
            }
                
            Transform passpointEx = doorExit.Find("DoorPoint");
            Vector2 exitPoint = passpointEx.position;
            
            if (!allAtDoorExit)
            {
                allAtDoorExit = true;
                foreach (GameObject hero in heroList)
                {
                    float dist = Vector2.Distance(hero.GetComponent<Rigidbody2D>().position, exitPoint);

                    if (dist > 0.05f)
                    {
                        bool result = hero.transform.GetComponent<HeroAI>().MoveToDoor(exitPoint);
                        if (!result)
                        {
                            allAtDoorExit = false;
                        }
                    }
                }
            }
            else
            {
                if (currentRoomId == roomList.Count() - 1)
                {
                    SceneManager.LoadSceneAsync("BossGym");
                }
                else
                {
                    Transform doorEntrance = roomList[currentRoomId + 1].transform.Find("EntranceDoor");
                    Transform passpointEn = doorEntrance.Find("DoorPoint");
                    Vector2 EntrancePoint = passpointEn.position;

                    allAtDoorEntrance = true;
                    foreach (GameObject hero in heroList)
                    {
                        float dist = Vector2.Distance(hero.GetComponent<Rigidbody2D>().position, EntrancePoint);

                        if (dist > 0.05f)
                        {
                            bool result = hero.transform.GetComponent<HeroAI>().MoveToDoor(EntrancePoint);
                            if (!result)
                            {
                                allAtDoorEntrance = false;
                            }
                        }
                    }
                    if (allAtDoorEntrance)
                    {
                        allAtDoorEntrance = false;
                        allAtDoorExit = false;
                        roomFinished = false;
                        currentRoomId++;
                    }

                }
            }
        }
    }
}
