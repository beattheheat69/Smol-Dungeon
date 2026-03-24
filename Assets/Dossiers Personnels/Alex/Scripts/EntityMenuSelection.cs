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

		//Crée des children pour chaque rooms dans le gameobject qui sera envoyé à la prochaine scene
		//Crée des child dans chaque rooms pour les monstres et les traps
		//Désactive par défaut car seront activé dans les rooms
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
			//References a limite par room et au script de gestion de Evil Points
			RoomEntityLimit roomLimit = toggle.GetComponent<RoomEntityLimit>();
			GlobalRessources globalRessources = GetComponent<GlobalRessources>();

			//Affect seulement le toggle selectionné
			if (toggle.GetComponent<Toggle>().isOn)
			{
				if (roomLimit.totalLimit > 0 && globalRessources.EvilPointsAmount() > 0)
				{
					if ((entity.CompareTag("Monster") || entity.CompareTag("TriggerMonster")) && roomLimit.monsterLimit > 0)
					{
						//Cherche la parent MonsterGroup et instantiate le monstre dedans
						foreach (Transform child in placeInRooms.WhatToSendOver.transform)
						{
							if (toggle.name == child.name)
								Instantiate(entity, child.Find("MonsterGroup"));
						}

						//Diminue limite de monstres, limite de la room, et Evil Point de 1
						roomLimit.monsterLimit--;
						roomLimit.totalLimit--;
						globalRessources.SpendEvilPoints(1);
					}
					if (entity.CompareTag("Trap") && roomLimit.trapLimit > 0)
					{
						//Cherche le parent TrapGroup et instantiate la trap dedans
						foreach (Transform child in placeInRooms.WhatToSendOver.transform)
						{
							if (toggle.name == child.name)
								Instantiate(entity, child.Find("TrapGroup"));
						}

						//Diminue limite de traps, limite de la room, et Evil Point de 1
						roomLimit.trapLimit--;
						roomLimit.totalLimit--;
						globalRessources.SpendEvilPoints(1);
					}
				}
			}
		}
	}
}
