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
    protected SoundCaster soundCaster; //Sound script common for all characters
    public Vector2 lastMoveDirection; // Direction the hero is looking for the attack

    void Awake()
    {
        animator = GetComponent<Animator>();
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        soundCaster = GetComponent<SoundCaster>();
    }

    //Take dammage when hit
    public virtual void takeDamage(int damage, Vector2 attackerPosition, float knockbackStrength)
    {

        animator.SetTrigger("Damaged");
        
        // deduct health
        health -= damage;

        if (this.gameObject.tag != "TriggerMonster") // Decided living Armor has no knockback, we can change that
        {
            animator.SetInteger("HP", health);
            rb.linearDamping = 10f;
            //Does Kockback to character with force of attacker
            Vector2 knockbackDir = ((Vector2)transform.position - attackerPosition).normalized;
            // Apply the custom force from the monster
            rb.AddForce(knockbackDir * knockbackStrength, ForceMode2D.Impulse);
            StartCoroutine(ResetDamping());
            atTarget = false;
        }
        else 
        {
            animator.SetTrigger("Damaged");
        }

        //Check if dead
        if (health <= 0 && !isDead)
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero; // Kill any sliding momentum
            if (this.gameObject.tag == "TriggerMonster")
            {
                animator.SetBool("Defeat", true);
            }
            Die();
        }

        //Calls hit SFX
        if (soundCaster != null)
        {
            Debug.Log("Playing Hit SFX");
            soundCaster.PlayHitSFX();
        }
    }

    public bool IsAlive()
    {
        return !isDead;
    }

    //Trigger death, deactivate character (tempo)
    protected void Die()
    {
        if(this.gameObject.tag != "Hero")
        {
            RoomInstance roomScript = transform.GetComponentInParent<RoomInstance>();
            roomScript.removeMonster(this.gameObject);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else if (this.gameObject.tag == "Hero")
        {
            if (gameObject.TryGetComponent<HeroAI>(out var ai))
            {
                ai.enabled = false;
            }
            else 
            {
                gameObject.GetComponent<HeroBossAI>().enabled = false;
            }
                
            gameObject.GetComponent<HeroAnimation>().enabled = false;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            HeroDataManager.Instance.NextDay();
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
