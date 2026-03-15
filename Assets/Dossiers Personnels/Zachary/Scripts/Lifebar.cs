using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    [Header("Position")]
    public float verticalOffset = 0f;

    [Header("Health Values")]
    public float maxHealth;
    public float currentHealth;

    public Slider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Make the player unable to change the value of the slider
        slider.interactable = false;

        //Set the max value of the Slider
        SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //Make the health bar appear over the entity's head whenever it's attacked?

        //Update the current health (FOR DEBUG PURPOSES)
        SetHealth(currentHealth);
    }

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth (float health)
    {
        slider.value = health;
    }

    public void SetAsActive(bool active)
    {
        slider.GameObject().SetActive(active);
    }
}
