using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroDataManager : MonoBehaviour
{
    public static HeroDataManager Instance { get; private set; }
    public HeroData[] party = new HeroData[2]; // Array of heros stats data
    int currentDay = 1;
    [SerializeField]
    public GameObject[] heroes = new GameObject[2];

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHeroHealh(int hero, int damage, int day)
    {
        party[hero].currentHealt -= damage;

        //End run when hero is dead
        if (party[hero].currentHealt <= 0)
            GameObject.Find("GameManager").GetComponent<RunStatus>().CallRestart(true, day);
    }

    public int GetHealt(int hero)
    {
        return party[hero].currentHealt;
    }
    public float GetDodgheChance(int hero)
    {
        return party[hero].dodgeChance;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "PlacementEntite")
        {    
            ActivateHeroForDay(); 
        }
        
    }

    public void ActivateHeroForDay()
    {

        // Activate the hero for this day
        int index = currentDay - 1;

        if (index >= 0 && index < heroes.Length)
            heroes[index].SetActive(true);
    }

    public void NextDay()
    {
        currentDay++;
    }

    public int GetDay()
    {
        return currentDay;
    }
}


public class HeroData
{
    public int currentHealt;
    public float dodgeChance;
    //public int currentNbPotion;
    //buffs and debuffs
}
