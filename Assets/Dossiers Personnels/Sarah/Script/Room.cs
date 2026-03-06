using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    GameObject enemyGroup; //Parent of all enemies
    private List<GameObject> monsterList = new List<GameObject>(); // List of all monster in room


    private void Awake()
    {
        //Fill list of all monsters in room
        foreach (Transform child in enemyGroup.transform)
        {
            monsterList.Add(child.gameObject);
        }
    }

    //If Hero enters room activate enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if object entering is a hero
        if (collision.CompareTag("Hero"))
        {
            if (monsterList.Count > 0)
            {
                enemyGroup.SetActive(true);
            }

            //if trap activate
        }
    }

    //If Hero enters room deactivate enemy
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Check if object entering is a hero
        if (collision.CompareTag("Hero"))
        {
            //if trap deactivate
        }
    }

    public List<GameObject> GetList()
    {
        return monsterList;
    }
}
