using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public int points;
    public string selectedEntity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectEntity(string entity)
    {
        selectedEntity = entity;
    }
}
