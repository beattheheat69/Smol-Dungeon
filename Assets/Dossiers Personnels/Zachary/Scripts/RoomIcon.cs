using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region EntityIcon Class
//A simple class to match the tag of an entity to a color (and potentially an image)
[System.Serializable]
public class EntityIcon
{
    public string entityTag;
    public Color entityColor;

    public EntityIcon(string tag, Color color)
    {
        entityTag = tag;
        entityColor = color;
    }
}
#endregion

public class RoomIcon : MonoBehaviour
{
    [SerializeField] private List<GameObject> iconObjects; //A list containing every GameObject used to hold the icon
    [SerializeField] private EntityIcon_SO entityIcons; //The scriptable object containing every EI object
    
    private int nextIconIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize both lists

        //iconObjects.Clear();
        //iconObjects.AddRange(GameObject.FindGameObjectsWithTag("RoomIcon")); (plan to eventually introduce a new Tag)
    }

    public void UpdateIcon(string tag)
    {
        //Get the correct icon from the tag
        foreach (EntityIcon icon in entityIcons.iconsList)
        {
            if (icon.entityTag == tag)
            {
                AddIcon(icon, nextIconIndex);
                break;
            }
        }

        //Update the index to reference the next available icon slot
        if (nextIconIndex - 1 > iconObjects.Count) //failsafe for index overflow
        {
            Debug.Log("Out of icons");
        }
        else
        {
            nextIconIndex++;
        }
    }

    void AddIcon(EntityIcon icon, int index)
    {
        iconObjects[index].GetComponent<Image>().color = icon.entityColor;
    }

    public void ClearAllIcons()
    {
        foreach (GameObject iconObject in iconObjects)
        {
            iconObject.GetComponent<Image>().color = Color.white;
        }

        nextIconIndex = 0;
    }
}
