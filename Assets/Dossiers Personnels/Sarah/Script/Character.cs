using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField]//testing
    protected int health; //enemy current health
    protected int power; //enemy urrent power
    protected bool isDead;
    protected Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        isDead = false;
    }

    //Take dammage when hit
    public virtual void takeDamage(int damage)
    {
        animator.SetTrigger("Damaged");
        
        // deduct health
        health -= damage;
        animator.SetInteger("HP", health);
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
        RoomInstance roomScript = transform.GetComponentInParent<RoomInstance>();
        roomScript.removeMonster(this.gameObject);
    }
}
