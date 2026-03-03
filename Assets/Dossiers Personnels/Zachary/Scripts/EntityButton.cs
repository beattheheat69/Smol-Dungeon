using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class EntityButton : MonoBehaviour
{
    public string entityName = "Entity";
    public int entityLevel = 0;
    public int entityPointCost = 1;

    public TextMeshProUGUI textElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textElement.text = entityName + "<br>LV:" + entityLevel + " EP:" + entityPointCost;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText()
    {
        textElement.text = entityName + "<br>LV:" + entityLevel + " EP:" + entityPointCost;
    }
}
