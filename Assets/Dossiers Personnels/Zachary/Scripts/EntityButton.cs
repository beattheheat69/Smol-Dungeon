using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class EntityButton : MonoBehaviour
{
    public string entityName = "Entity";
    public int entityLevel = 0;
    public int entityPointCost = 1;

    public GameObject cursorTextBox;

    [HideInInspector]
    public TextMeshProUGUI cursorTextElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //cursorTextBox = GameObject.Find("CursorTextBox");
        //Note: I'd eventually like for the button to find the CursorTextBox automatically to be compatible with the Prefab
        cursorTextElement = cursorTextBox.GetComponentInChildren<TextMeshProUGUI>();
        cursorTextBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText()
    {
        //Set the text of the text box to display the stats of the current entity
        cursorTextElement.SetText(entityName + "<br>LV:" + entityLevel + " EP:" + entityPointCost);
    }

    public void ToggleTextBox(Boolean state)
    {
        cursorTextBox.SetActive(state);
    }
}
