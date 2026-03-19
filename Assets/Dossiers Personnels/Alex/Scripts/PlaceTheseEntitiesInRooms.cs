using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaceTheseEntitiesInRooms : MonoBehaviour
{
    EntityMenuSelection entitySelection;
    public GameObject WhatToSendOver;
    public List<EachRoomList> everyRoomEntities = new List<EachRoomList>();
    public EachRoomList allRooms;

	private void Start()
	{
		entitySelection = GetComponent<EntityMenuSelection>();
        DontDestroyOnLoad(WhatToSendOver);
	}

	public void ConfirmChoices()
    {
        ////Check if all lists are OK first before allowing scene transition

        foreach (Toggle toggle in entitySelection.RoomButtons)
        {
			everyRoomEntities.Add(new EachRoomList());
			for (int i = 0; i < toggle.gameObject.transform.childCount; i++)
			{
				if (toggle.gameObject.transform.GetChild(i).CompareTag("Monster"))
				{
					//allRooms.Monsters.Add(child.gameObject);
					everyRoomEntities[entitySelection.RoomButtons.IndexOf(toggle)].Monsters.Add(toggle.gameObject.transform.GetChild(i).gameObject);
				}
				else if (toggle.gameObject.transform.GetChild(i).CompareTag("Trap"))
				{
					//allRooms.Traps.Add(child.gameObject);
					everyRoomEntities[entitySelection.RoomButtons.IndexOf(toggle)].Traps.Add(toggle.gameObject.transform.GetChild(i).gameObject);
				}
			}
			//foreach (Transform child in toggle.transform)
			//{
			//	if (child.CompareTag("Monster"))
			//	{
			//		//allRooms.Monsters.Add(child.gameObject);
			//		everyRoomEntities[entitySelection.RoomButtons.IndexOf(toggle)].Monsters.Add(child.gameObject);
			//	}
			//	else if (child.CompareTag("Trap"))
			//	{
			//		//allRooms.Traps.Add(child.gameObject);
			//		everyRoomEntities[entitySelection.RoomButtons.IndexOf(toggle)].Traps.Add(child.gameObject);
			//	}
			//}
			//everyRoomEntities.Add(allRooms); //Bug: total is added to each room, Clear() does not fix it
		}

        StartCoroutine(GoToDungeon());
    }

    IEnumerator GoToDungeon()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("PremadeDungeonTest");
    }
}

[System.Serializable]
public class EachRoomList
{
    public List<GameObject> Monsters;
	public List<GameObject> Traps;
}