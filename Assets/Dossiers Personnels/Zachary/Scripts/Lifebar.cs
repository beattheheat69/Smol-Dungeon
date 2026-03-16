using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    [Header("Health Values")]
    public float maxHealth;
    public float currentHealth;

    [Header("Color")]
    public Gradient gradient;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Bound the health between 0 and the max health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        SetHealth(currentHealth);
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        currentHealth = health;
        slider.maxValue = maxHealth;
        SetHealth(maxHealth);
    }

    public void SetHealth(float health)
    {
        //Set the value of the slider to the current health
        slider.value = health;

        //Set the color of the slider according to the remaining health
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetAsActive(bool active)
    {
        slider.GameObject().SetActive(active);
    }
}
