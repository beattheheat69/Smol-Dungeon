using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hero : Character
{
    [SerializeField]
    protected HeroStats_SO baseStats; // Base stats of heros
    [SerializeField]
    protected int index;
    

    //Rigidbody2D rb;  //Object rigidbody

    private void Awake()
    {;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public override void takeDamage(int damage, Vector2 attackerPosition, float knockbackStrength)
    {
        float randVal = Random.Range(1, 100);

        if (randVal > HeroDataManager.Instance.GetDodgheChance(index))
        {
            rb.linearDamping = 10f;
            //Debug.Log("<color=cyan><b>[pushed]</b> Hero got hit and knockback</color>");
            // deduct health
            HeroDataManager.Instance.UpdateHeroHealh(index, damage, index+1);
            //Does Kockback to character with force of attacker
            Vector2 knockbackDir = ((Vector2)transform.position - attackerPosition).normalized;
            rb.AddForce(knockbackDir * knockbackStrength, ForceMode2D.Impulse);
            StartCoroutine(ResetDamping());
            atTarget = false;

            health = HeroDataManager.Instance.party[index].currentHealt;  // testing hero health

            animator.SetTrigger("Damaged");

			//Damage number anim
			damageNumberAnim.GetComponentInChildren<TextMesh>().text = damage.ToString();
			damageNumberAnim.GetComponentInChildren<TextMesh>().color = Color.red;
			GameObject inst = Instantiate(damageNumberAnim, transform.position, Quaternion.identity);
			Destroy(inst, 1.0f);
		}
        else
        {
			//Calls miss anim and text on enemy
			damageNumberAnim.GetComponentInChildren<TextMesh>().text = "Block";
			damageNumberAnim.GetComponentInChildren<TextMesh>().color = Color.red;
			GameObject instan = Instantiate(damageNumberAnim, transform.position, Quaternion.identity);
			Destroy(instan, 1f);
			//Call target block anim and sfx
			animator.SetTrigger("Block");
			GetComponent<SoundCaster>().PlayBlockSFX();
		}

        //Check if dead
        if (HeroDataManager.Instance.party[index].currentHealt <= 0)
        {
            animator.SetBool("Defeat", true);
            base.Die();
        }

		GetComponent<SoundCaster>().PlayHitSFX();

		//Hit stop
		//StartCoroutine(HandyFunctions.HitStop());
	}

    IEnumerator ResetDamping()
    {
        isStunned = true;
        yield return new WaitForSeconds(0.4f);
        rb.linearVelocity = Vector2.zero;
        rb.linearDamping = 0f; // Go back to normal walking
        isStunned = false;
    }
}
