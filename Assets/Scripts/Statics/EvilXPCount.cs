using System;
using UnityEngine;

public class EvilXPCount : MonoBehaviour
{
    public static int EvilXP = 0;
    public static Action<int> ExpAmountChange;

    public static bool[] upgrades = new bool[]{true,false, true, false };

    public static void GainXP(int amount)
    {
        EvilXP += amount;
    }

    public static int GetXP()
    {
        return EvilXP;
    }

    public static void SpendEXP(int amount)
    {
        EvilXP -= amount;
        ExpAmountChange(EvilXP);
    }

    
}
