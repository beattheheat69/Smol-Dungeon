using UnityEngine;

public class Hero : Character
{
    [SerializeField]
    protected PaladinStats_SO baseStats; // Base stats of heros
    protected int index;

    public override void takeDamage(int damage)
    {
        float randVal = Random.Range(1, 100);

        if(randVal < HeroDataManager.Instance.GetDodgheChance(index))
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
