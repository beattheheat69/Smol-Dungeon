using System.Collections.Generic;
using UnityEngine;

public class RoomInstance : MonoBehaviour
{
    [SerializeField]
    GameObject enemyGroup; //Parent of all enemies in room
    [SerializeField]
    GameObject trapGroup; // Parent of all traps in room
    List<GameObject> monsterList = new List<GameObject>(); // List of all monster in room
    List<GameObject> trapList = new List<GameObject>();// List of all traps in room

    private void Awake()
    {
        if (transform.Find("CameraPoint") != null)
        {
            if (enemyGroup != null)
            {
                //Fill list of all monsters in room
                foreach (Transform child in enemyGroup.transform)
                {
                    monsterList.Add(child.gameObject);
                }
            }

            if (trapGroup != null)
            {
                //Fill list of all monsters in room
                foreach (Transform child in trapGroup.transform)
                {
                    trapList.Add(child.gameObject);
                }
            }

        }
    }

    //If Hero enters room activate enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if object entering is a hero
        if (collision.CompareTag("Hero"))
        {
            CameraManagement cameraManager = Camera.main.GetComponent<CameraManagement>();
            cameraManager.MoveToNewRoom(this.transform.Find("CameraPoint").transform.position);
            foreach (Transform child in transform)
            {   
                if(child.CompareTag("Group"))
                {
                    child.gameObject.SetActive(true);
                }             
            }
        }
    }

    //If Hero enters room deactivate enemy
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Check if object entering is a hero
        if (collision.CompareTag("Hero"))
        {
            //Check if object entering is a hero
            if (collision.CompareTag("Hero"))
            {
                foreach (Transform child in transform)
                {
                    if (child.CompareTag("Group"))
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public List<GameObject> GetList()
    {
        return monsterList;
    }
}
