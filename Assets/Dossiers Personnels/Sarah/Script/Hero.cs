using UnityEngine;

public class Hero : Character
{
    [SerializeField]
    protected PaladinStats_SO baseStats; // Base stats of heros

    public new void takeDamage(int damage)
    {
        float randVal = Random.Range(1, 100);
        //Check if hero dodge attack
        if(randVal > baseStats.dodgeChange)
        {
            // deduct health
            health -= damage;
        }

        //Check if dead
        if (health <= 0)
        {
            base.Die();
        }
    }


}
