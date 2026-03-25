using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Hero : Character
{
    [SerializeField]
    protected PaladinStats_SO baseStats; // Base stats of heros
    protected int index;
    //Rigidbody2D rb;  //Object rigidbody

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void takeDamage(int damage, Vector2 attackerPosition, float knockbackStrength)
    {
        float randVal = 100; //Random.Range(1, 100);

        if (randVal > HeroDataManager.Instance.GetDodgheChance(index))
        {
            rb.linearDamping = 10f;
            Debug.Log("Hero got hit by : " + damage); // teste hero getting hit
            // deduct health
            HeroDataManager.Instance.UpdateHeroHealh(index, damage);

            //Does Kockback to character with force of attacker
            Vector2 knockbackDir = ((Vector2)transform.position - attackerPosition).normalized;
            rb.AddForce(knockbackDir * knockbackStrength, ForceMode2D.Impulse);
            StartCoroutine(ResetDamping());
            atTarget = false;

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

    IEnumerator ResetDamping()
    {
        yield return new WaitForSeconds(0.3f);
        rb.linearDamping = 0f; // Go back to normal walking
    }
}
