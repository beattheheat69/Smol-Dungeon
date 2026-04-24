using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UIElements;

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
    [SerializeField]
    int id;

    void OnEnable()
    {
        GlobalRessources.PAmountChange += CheckPoints;
    }

    void OnDisable()
    {
        GlobalRessources.PAmountChange -= CheckPoints;
    }


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

        if (!EvilXPCount.upgrades[id])
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckPoints(int eveilPoint)
    {
        if (eveilPoint >= entityCost && entityCost > 0)
            transform.GetComponent<UnityEngine.UI.Button>().interactable = true;
        else if (entityCost > 0)
            transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
    }

    public void ToggleTextBox(bool state)
    {
        flavorTextElement.SetText(flavorText);
        flavorTextBox.SetActive(state);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleTextBox(true);
        //Debug.Log("Entered a button");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleTextBox(false);
        //Debug.Log("Exited a button");
    }
}
