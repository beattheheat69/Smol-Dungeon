using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class EntityMenuSelection : MonoBehaviour
{
    public GameObject Map;
    public List<GameObject> RoomButtons = new List<GameObject>();
	public List<GameObject> Entities;

	private void Start()
	{
		foreach (Toggle toggle in Map.GetComponentsInChildren<Toggle>())
		{
			if (toggle.interactable == true)
				RoomButtons.Add(toggle.gameObject);
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

				//Check for limit per room
				if (roomLimit.totalLimit > 0 && globalRessources.EvilPointsAmount() > 0)
				{
					if (entity.CompareTag("Monster") && roomLimit.monsterLimit > 0)
					{
						Instantiate(entity, toggle.transform).SetActive(false);
						roomLimit.monsterLimit--;
						roomLimit.totalLimit--;
						globalRessources.SpendEvilPoints(1);
					}
					if (entity.CompareTag("Trap") && roomLimit.trapLimit > 0)
					{
						Instantiate(entity, toggle.transform).SetActive(false);
						roomLimit.trapLimit--;
						roomLimit.totalLimit--;
						globalRessources.SpendEvilPoints(1);
					}
				}
			}
		}
	}
}
