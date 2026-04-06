using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityMenuSelection : MonoBehaviour
{
    public GameObject Map;
    public List<GameObject> RoomButtons = new List<GameObject>();
	public List<GameObject> Entities;
	PlaceTheseEntitiesInRooms placeInRooms;

	[Header("Prefabs Reference")]
	public GameObject slimePrefab;
	public GameObject armourPrefab;
	public GameObject crossbowPrefab;
	public GameObject spikesPrefab;

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
				if (roomLimit.currentTotalCount > 0 && globalRessources.EvilPointsAmount() > 0)
				{
					if ((entity == slimePrefab || entity == armourPrefab) && roomLimit.currentMonsterCount > 0)
					{
						//Cherche la parent MonsterGroup et instantiate le monstre dedans
						foreach (Transform child in placeInRooms.WhatToSendOver.transform)
						{
							if (toggle.name == child.name)
								Instantiate(entity, child.Find("MonsterGroup"));
						}

						//Diminue limite de monstres, limite de la room, et Evil Point de 1
						//roomLimit.monsterLimit--;
						//roomLimit.totalLimit--;
						roomLimit.OccupyMonsterSpot();
						//globalRessources.SpendEvilPoints(1);
						globalRessources.SpendEvilPoints(entity.GetComponent<MonsterAction>().baseStats.cost); //Use this one when Evil Point Cost added to SO

						if (entity == slimePrefab)
                            toggle.GetComponent<RoomIcon>().UpdateIcon("Slime"); //Ajuster l'icône de la chambre
						if (entity == armourPrefab)
                            toggle.GetComponent<RoomIcon>().UpdateIcon("Armour"); //Ajuster l'icône de la chambre
                    }
					if (entity == crossbowPrefab && roomLimit.currentTrapCount > 0)
					{
						//Cherche le parent TrapGroup et instantiate la trap dedans
						foreach (Transform child in placeInRooms.WhatToSendOver.transform)
						{
							if (toggle.name == child.name)
								Instantiate(entity, child.Find("TrapGroup"));
						}

						//Diminue limite de traps, limite de la room, et Evil Point de 1
						//roomLimit.trapLimit--;
						//roomLimit.totalLimit--;
						roomLimit.OccupyTrapSpot();
						//globalRessources.SpendEvilPoints(1);
						globalRessources.SpendEvilPoints(entity.GetComponent<CrossbowAI>().baseStat.cost); //Use this one when Evil Point Cost added to SO

						toggle.GetComponent<RoomIcon>().UpdateIcon("Crossbow"); //Ajuster l'icône de la chambre
					}
					if (entity == spikesPrefab && roomLimit.currentTrapCount > 0 && roomLimit.currentSpikesCount > 0)
					{
						//Cherche le parent TrapGroup et instantiate la trap dedans
						foreach (Transform child in placeInRooms.WhatToSendOver.transform)
						{
							if (toggle.name == child.name)
								Instantiate(entity, child.Find("TrapGroup"));
						}

						//Diminue limite de traps, limite de la room, et Evil Point de 1
						//roomLimit.trapLimit--;
						//roomLimit.spikesLimit--;
						//roomLimit.totalLimit--;
						roomLimit.OccupySpikesSpot();
						//globalRessources.SpendEvilPoints(1);
						globalRessources.SpendEvilPoints(entity.GetComponent<SpikeAI>().baseStats.cost); //Use this one when Evil Point Cost added to SO

						toggle.GetComponent<RoomIcon>().UpdateIcon("Spike"); //Ajuster l'icône de la chambre
                    }
				}
			}
		}
	}

	public void ClearRoomEntities()
	{
		foreach (GameObject toggle in RoomButtons)
		{
			RoomEntityLimit roomLimit = toggle.GetComponent<RoomEntityLimit>();
			GlobalRessources globalRessources = GetComponent<GlobalRessources>();
			if (toggle.GetComponent<Toggle>().isOn)
			{
				int costValueInRoom = 0;
				foreach (Transform child in placeInRooms.WhatToSendOver.transform)
				{
					if (toggle.name == child.name)
					{
						//Get cost value of each entities in each MonsterGroup and TrapGroup
						//Destroy objects in MonsterGroup and TrapGroup
						//Need to find how to remove icons from toggle button
						foreach (Transform child2 in child.Find("MonsterGroup").transform)
						{
							costValueInRoom += child2.GetComponent<MonsterAction>().baseStats.cost;
							Destroy(child2.gameObject);
						}
						foreach (Transform child2 in child.Find("TrapGroup").transform)
						{
							if (child2.GetComponent<CrossbowAI>() != null)
								costValueInRoom += child2.GetComponent<CrossbowAI>().baseStat.cost;
							else if (child2.GetComponent<SpikeAI>() != null)
								costValueInRoom += child2.GetComponent<SpikeAI>().baseStats.cost;
							Destroy(child2.gameObject);
						}
					}
				}
				globalRessources.GainEvilPoints(costValueInRoom);
				roomLimit.ClearRoom();
				toggle.GetComponent<RoomIcon>().ClearAllIcons();
			}
		}
	}
}
