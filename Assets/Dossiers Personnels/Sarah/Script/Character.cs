using UnityEngine;

public class Character : MonoBehaviour
{
    protected int health; //enemy current health
    protected int power; //enemy urrent power

    //Take dammage when hit
    public void takeDamage(int damage)
    {
        // deduct health
        health -= damage;

        //Check if dead
        if (health <= 0)
        {
            Die();
        }
    }

    //Trigger death, deactivate character (tempo)
    protected void Die()
    {
        gameObject.SetActive(false);
    }
}
