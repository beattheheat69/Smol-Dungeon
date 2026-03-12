using UnityEngine;

public class Hero : Character
{
    [SerializeField]
    protected PaladinStats_SO baseStats; // Base stats of heros
    protected int index;

    public new void takeDamage(int damage)
    {
        Debug.Log("Hero got it");
        float randVal = Random.Range(1, 100);
        //Check if hero dodge attack
        if(randVal < baseStats.dodgeChange)
        {
            // deduct health
            HeroDataManager.Instance.UpdateHeroHealh(index, damage);
        }

        //Check if dead
        if (HeroDataManager.Instance.party[index].currentHealt<= 0)
        {
            base.Die();
        }
    }
}
