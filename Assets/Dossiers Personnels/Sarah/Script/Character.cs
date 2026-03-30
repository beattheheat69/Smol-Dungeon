using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField]//testing
    protected int health; //enemy current health
    protected int power; //enemy urrent power
    protected bool isDead;
    protected bool atTarget = false;
    protected bool isStunned = false;
    protected Animator animator;
    protected Rigidbody2D rb;  //Object rigidbody
    GameManager gm;

    void Awake()
    {
        gm = FindFirstObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
    }

    //Take dammage when hit
    public virtual void takeDamage(int damage, Vector2 attackerPosition, float knockbackStrength)
    {
        if (this.gameObject.tag != "TriggerMonster") // remove if when living Armor has animation
        {
             animator.SetTrigger("Damaged");
        }
       
        
        // deduct health
        health -= damage;
        if (this.gameObject.tag != "TriggerMonster") // remove if when living Armor has animation
        {
            animator.SetInteger("HP", health);
        }

        if (this.gameObject.tag != "TriggerMonster") // Decided living Armor has no knockback, we can change that
        {
            rb.linearDamping = 10f;
            //Does Kockback to character with force of attacker
            Vector2 knockbackDir = ((Vector2)transform.position - attackerPosition).normalized;
            // Apply the custom force from the monster
            rb.AddForce(knockbackDir * knockbackStrength, ForceMode2D.Impulse);
            StartCoroutine(ResetDamping());
            atTarget = false;
        }

        //Check if dead
        if (health <= 0)
        {
            //animator.SetBool("Defeated", true);
            isDead = true;
            Die();
        }
    }

    public bool IsAlive()
    {
        return !isDead;
    }

    //Trigger death, deactivate character (tempo)
    protected void Die()
    {

        if (this.gameObject.tag == "TriggerMonster")// remove when living armor has animation
        {
            gameObject.SetActive(false);
        }
        else if(this.gameObject.tag != "Hero")
        {
            RoomInstance roomScript = transform.GetComponentInParent<RoomInstance>();
            roomScript.removeMonster(this.gameObject);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        else if (this.gameObject.tag == "Hero")
        {
            gameObject.GetComponent<HeroAI>().enabled = false;
            gameObject.GetComponent<HeroAnimation>().enabled = false;

            gm.NextDay();
        }

    }


    IEnumerator ResetDamping()
    {
        //isStunned = true;

        yield return new WaitForSeconds(0.3f);
        rb.linearDamping = 0f; // Go back to normal walking

        //isStunned = false;
    }
}
