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
    }

    public int GetHealt(int hero)
    {
        return party[hero].currentHealt;
    }

}


public class HeroData
{
    public int currentHealt;
    //public int currentNbPotion;
    //buffs and debuffs
}
