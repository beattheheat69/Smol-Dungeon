using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaceTheseEntitiesInRooms : MonoBehaviour
{
    EntityMenuSelection entitySelection;
    public List<GameObject> RoomsInDungeon = new List<GameObject>();
    public GameObject WhatToSendOver;

	private void Start()
	{
		entitySelection = GetComponent<EntityMenuSelection>();
        DontDestroyOnLoad(WhatToSendOver);
	}

	public void ConfirmChoices()
    {
        //Check if all lists are OK first before allowing scene transition
        //Maybe change scene in coroutine to allow some time for loading and affecting gameobjects in lists
        foreach (Toggle toggle in entitySelection.RoomButtons)
        {
            RoomsInDungeon.Add(toggle.gameObject);
        }

        for (int i = 0; i < RoomsInDungeon.Count; i++)
        {
            GameObject inst = Instantiate(RoomsInDungeon[i], WhatToSendOver.transform);
            foreach(Transform child in RoomsInDungeon[i].GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("UI"))
                    Destroy(child); //Doesn't work, can't remove recttransform
                else if (child.CompareTag("Monster"))
                {
                    //Put in monster list
                }
                else if (child.CompareTag("Trap"))
                {
                    //Put in trap list
                }
            }
        }

        StartCoroutine(GoToDungeon());
    }

    IEnumerator GoToDungeon()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("PremadeDungeonTest");
    }
}