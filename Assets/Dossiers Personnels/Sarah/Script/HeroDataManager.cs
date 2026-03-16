using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HeroDataManager : MonoBehaviour
{
    public static HeroDataManager Instance { get; private set; }
    public List <HeroData> party = new List<HeroData> (); // list of heros stats data

    void Awake()
    {
        if(Instance != null && Instance != this )
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateHeroHealh(int hero, int damage)
    {
        party[hero].currentHealt -= damage;

        //End run when hero is dead
        if (party[hero].currentHealt <= 0)
            GetComponent<RunStatus>().CallRestart(true);
    }

    public int GetHealt(int hero)
    {
        return party[hero].currentHealt;
    }
    public float GetDodgheChance(int hero)
    {
        return party[hero].dodgeChance;
    }

}


public class HeroData
{
    public int currentHealt;
    public float dodgeChance;
    //public int currentNbPotion;
    //buffs and debuffs
}
