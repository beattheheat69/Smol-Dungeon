using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlaceTheseEntitiesInRooms : MonoBehaviour
{
    EntityMenuSelection entitySelection;
    public GameObject WhatToSendOver;
    public List<EachRoomList> everyRoomEntities = new List<EachRoomList>();
    public EachRoomList allRooms;
	GlobalRessources globalRessources;

	private void Start()
	{
		entitySelection = GetComponent<EntityMenuSelection>();
		globalRessources = GetComponent<GlobalRessources>();
        DontDestroyOnLoad(WhatToSendOver);
	}

	public void ConfirmChoices()
    {
		//foreach (GameObject toggle in entitySelection.RoomButtons)
		//{
		//	foreach (Transform child in toggle.transform)
		//	{
		//		if (child.CompareTag("Monster"))
		//		{
		//			allRooms.Monsters.Add(child.gameObject);
		//			//everyRoomEntities[entitySelection.RoomButtons.IndexOf(toggle)].Monsters.Add(child.gameObject);
		//		}
		//		else if (child.CompareTag("Trap"))
		//		{
		//			allRooms.Traps.Add(child.gameObject);
		//			//everyRoomEntities[entitySelection.RoomButtons.IndexOf(toggle)].Traps.Add(child.gameObject);
		//		}
		//	}
		//	everyRoomEntities.Add(allRooms); //Bug: total is added to each room, Clear() does not fix it
		//}

		//for (int i = 0; i < entitySelection.RoomButtons.Count; i++)
		//{
		//	everyRoomEntities.Add(new EachRoomList());
		//	for (int j = 0; j < entitySelection.RoomButtons[i].transform.childCount; j++)
		//	{
		//		GameObject objToAdd = entitySelection.RoomButtons[i].transform.GetChild(j).gameObject;
		//		print(objToAdd); //Bugs at the first obj I'm trying to grab (goes ok for background though) wtf, also doesn't matter if obj is inactive or not
		//		if (objToAdd.CompareTag("Monster"))
		//		{
		//			//Reaches here
		//			everyRoomEntities[i].Monsters[j] = new GameObject();
		//			print("Found a monster"); //But not here
		//			//everyRoomEntities[i].Monsters.Add(new GameObject());	//doesn't reach here either, thinks monster list is null??
		//			print(everyRoomEntities[i].Monsters[j]);
		//			//everyRoomEntities[i].Monsters.Add(objToAdd); //What's wrong here???
		//		}
		//		else if (objToAdd.CompareTag("Trap"))
		//		{
		//			everyRoomEntities[i].Traps.Add(objToAdd);
		//		}
		//	}
		//}

		StartCoroutine(GoToDungeon());
    }

    IEnumerator GoToDungeon()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadSceneAsync("PremadeDungeonTest");
    }
}

[System.Serializable]
public class EachRoomList
{
    public List<GameObject> Monsters;
	public List<GameObject> Traps;
}