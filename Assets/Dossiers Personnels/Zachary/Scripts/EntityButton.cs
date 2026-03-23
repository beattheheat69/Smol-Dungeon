using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class EntityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Entity Data")]
    public string entityName = "Entity Name";
    public int entityLevel = 0;
    public int entityCost = 1;
    public string flavorText = "Entity Flavor Text";

    [Header("Inheritances")]
    [SerializeField] public TextMeshProUGUI nameTMP;
    [SerializeField] public TextMeshProUGUI costTMP;

    [SerializeField] GameObject flavorTextBox;
    [HideInInspector] TextMeshProUGUI flavorTextElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set the text elements of the button
        nameTMP.SetText(entityName);
        costTMP.SetText(entityCost.ToString().PadLeft(2,'0'));

        //Set up the text box cursor
        //flavorTextBox = GameObject.FindWithTag("FlavorTextBox"); 
        flavorTextElement = flavorTextBox.GetComponentInChildren<TextMeshProUGUI>();
        flavorTextBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleTextBox(bool state)
    {
        flavorTextElement.SetText(flavorText);
        flavorTextBox.SetActive(state);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleTextBox(true);
        Debug.Log("Entered a button");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleTextBox(false);
        Debug.Log("Exited a button");
    }
}
