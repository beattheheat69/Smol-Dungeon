using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MonsterAI : Character, IDamageable
{
    [SerializeField]
    SlimeStats_SO baseStat; // Base stats of slimes

    GameObject target = null; // current hero target
    Rigidbody2D rb;  //Object rigidbody
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // monster in attack mode

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = baseStat.health;
        power = baseStat.power;
    }

    //Check for target, attack if possible
    private void FixedUpdate()
    {
        //If no target check for one
        if (target == null)
        {
            FindTarget();
        }

        //Move to target
        MoveEnemy();

        //Check if can attack
        if (attacking && timeCooldown <= 0)
        {
            DoAttack();

            //If target dead, find new one
            if (!CheckTargetAlive())
            {
                FindTarget();
                if (target == null)
                {
                    transform.parent.gameObject.SetActive(false);
                }
            }
        }

        //attack cooldown
        if (timeCooldown > 0)
        {
            timeCooldown -= Time.deltaTime;
        }
    }

    //Enter in attack mode when colliding with target
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target && !attacking)
        {
            attacking = true;
        }
    }

    //Leaves attack mode when not colliding with target
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.gameObject == target)
        {

            attacking = false;
        }
    }

    //Check if target still alive
    bool CheckTargetAlive()
    {
        return target.activeSelf;
    }

    //Does an attack with a range infront of him, according to his direction of movement
    private void DoAttack()
    {
        float randVal = Random.Range(1, 100);

        //Check is attack succeded
        if (randVal <= baseStat.attackChance)
        {
            IDamageable hitTarget = target.GetComponent<IDamageable>();
            hitTarget.takeDamage(power);
        }
        //Start cooldown
        timeCooldown = baseStat.attackCooldown;
    }

    //Move monster towards target
    private void MoveEnemy()
    {
        //caculate distance between monster and target with consistent speed
        Vector2 direction = (target.transform.position - transform.position).normalized;
        // Move monster toward target
        rb.MovePosition(rb.position + direction * baseStat.chargeSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) < 0.13f)
        {
            //Push back enemy if overlapping with hero
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, -baseStat.chargeSpeed * Time.deltaTime);
        }
    }

    //Find which hero is the closest
    private void FindTarget()
    {
        float closestDist = Mathf.Infinity; //smallest distance between monsters and hero
        GameObject nearTarget = null; //closest hero to monster

        //Check all hero to see which is closest
        foreach (GameObject hero in HeroParty.Instance.GetList())
        {
            //Check if hero is active
            if (hero.activeSelf)
            {
                float distance = Vector2.Distance(transform.position, hero.transform.position);
                if (distance < closestDist)
                {
                    closestDist = distance;
                    nearTarget = hero;
                }
            }
        }
        //Set the hero that is closest to enemy in global variable
        target = nearTarget;
    }
}
