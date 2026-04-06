using UnityEngine;

public class EvilXPCount : MonoBehaviour
{
    public static int EvilXP = 0;

    public static void GainXP(int amount)
    {
        EvilXP += amount;
    }

    public static int GetXP()
    {
        return EvilXP;
    }
}
