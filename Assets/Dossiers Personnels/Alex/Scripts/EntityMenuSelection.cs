using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class EntityMenuSelection : MonoBehaviour
{
    public GameObject Map;
    public List<GameObject> RoomButtons = new List<GameObject>();
	public List<GameObject> Entities;
	PlaceTheseEntitiesInRooms placeInRooms;

	private void Start()
	{
		placeInRooms = GetComponent<PlaceTheseEntitiesInRooms>();

		foreach (Toggle toggle in Map.GetComponentsInChildren<Toggle>())
		{
			if (toggle.interactable == true)
			{
				RoomButtons.Add(toggle.gameObject);
				GameObject roomParent = new GameObject(toggle.name);
				roomParent.transform.parent = placeInRooms.WhatToSendOver.transform;
				GameObject monsterParent = new GameObject("MonsterGroup");
				monsterParent.transform.parent = roomParent.transform;
				monsterParent.tag = "Group";
				monsterParent.SetActive(false);
				GameObject trapParent = new GameObject("TrapGroup");
				trapParent.transform.parent = roomParent.transform;
				trapParent.tag = "Group";
				trapParent.SetActive(false);
			}
		}
	}

	public void PlaceInThisRoom(GameObject entity)
	{
		foreach (GameObject toggle in RoomButtons)
		{
			RoomEntityLimit roomLimit = toggle.GetComponent<RoomEntityLimit>();
			GlobalRessources globalRessources = GetComponent<GlobalRessources>();

			if (toggle.GetComponent<Toggle>().isOn)
			{
				//Place whatever entity from the button clicked into the corresponding room of this toggle button
				//Instantiate as child of toggle then split them between Tags and make new lists for those
				//Place in separate gameobject to transfer?
				//Send to other script in static/dont destroy on load?
				//Lists on monsters and traps for each room?
				//Check aussi pour limite d'entites par room

				//Instantiate it directly in whattosendover object instead of in the canvas toggle buttons

				//Check for limit per room
				if (roomLimit.totalLimit > 0 && globalRessources.EvilPointsAmount() > 0)
				{
					if (entity.CompareTag("Monster") && roomLimit.monsterLimit > 0)
					{
						//Change: put it in child parent monster
						foreach (Transform child in placeInRooms.WhatToSendOver.transform)
						{
							if (toggle.name == child.name)
								Instantiate(entity, child.Find("MonsterGroup"));
						}

						roomLimit.monsterLimit--;
						roomLimit.totalLimit--;
						globalRessources.SpendEvilPoints(1);
					}
					if (entity.CompareTag("Trap") && roomLimit.trapLimit > 0)
					{
						//Change: put it in child parent trap
						foreach (Transform child in placeInRooms.WhatToSendOver.transform)
						{
							if (toggle.name == child.name)
								Instantiate(entity, child.Find("TrapGroup"));
						}

						roomLimit.trapLimit--;
						roomLimit.totalLimit--;
						globalRessources.SpendEvilPoints(1);
					}
				}
			}
		}
	}
}
