using UnityEngine;

public class Hero : Character
{
    [SerializeField]
    protected PaladinStats_SO baseStats; // Base stats of heros
    protected int index;

    public override void takeDamage(int damage, Vector2 attackerPosition)
    {
        float randVal = Random.Range(1, 100);

        if (randVal > HeroDataManager.Instance.GetDodgheChance(index))
        {
            Debug.Log("Hero got hit by : " + damage); // teste hero getting hit
            // deduct health
            HeroDataManager.Instance.UpdateHeroHealh(index, damage);
            health = HeroDataManager.Instance.party[index].currentHealt;  // testing hero health
        }
        else
        {
            Debug.Log("hero dodge attack"); //teste hero dodging the attack
        }
        //Check if dead
        if (HeroDataManager.Instance.party[index].currentHealt <= 0)
        {
            base.Die();
        }
    }
}
