using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SlimeAI : MonsterAI
{
    [SerializeField]
    SlimeStats_SO baseStats; // Base stats of slimes
    [SerializeField]
    LayerMask colliderLayer;
    CameraManagement cameraStat; // check the stat of the camera
    GameObject target = null; // current hero target
    //Rigidbody2D rb;  //Object rigidbody
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // monster in attack mode
    bool isJumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = baseStats.health;
        power = baseStats.power;
        cameraStat = Camera.main.GetComponent<CameraManagement>();
    }

    //Check for target, attack if possible
    private void FixedUpdate()
    {
        if (!isDead)
        {
            //If camera is moving do nothing
            if (cameraStat.GetTransitionning()) return;
            //If no target check for one
            if (target == null)
            {
                FindTarget();
            }

            if (!atTarget)
            {
                //Move to target
                MoveEnemy();
            }


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

    }

    //Enter in attack mode when colliding with target
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target && !attacking)
        {
            attacking = true;
        }
    }

    //Leaves attack mode when not colliding with target
    private void OnCollisionExit2D(Collision2D collision)
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

    //Does an attack on target
    private void DoAttack()
    {
        float randVal = Random.Range(1, 100);

        //Check is attack succeded
        if (randVal <= baseStats.attackChance)
        {
            Debug.Log("Attack normaly");
            if (target.TryGetComponent(out IDamageable hitTarget)) //BUG: One shots hero
            {
                hitTarget.takeDamage(baseStats.power, transform.position, 0f);
                animator.SetTrigger("Attack");
            }
        }
        //Start cooldown
        timeCooldown = baseStats.attackCooldown;
    }

    //Move monster towards target
    private void MoveEnemy()
    {
        if (isJumping) return;
        //caculate distance between hero and target with consistent speed
        Vector2 direction = (target.transform.position - transform.position).normalized;
        // Move hero toward target
        rb.MovePosition(rb.position + direction * baseStats.chargeSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) <= 3f && Vector2.Distance(transform.position, target.transform.position) > 1f)
        {
            StartCoroutine(BounceAttack());
            /* Vector2 pushDirect = ((Vector2)transform.position - (Vector2)target.transform.position).normalized;
             transform.position += (Vector3)pushDirect * baseStats.chargeSpeed * Time.deltaTime;*/
            timeCooldown = baseStats.attackCooldown;
            atTarget = true;
        }
        //Correctoverlap();
    }

    private void Correctoverlap()
    {
        Collider2D[] touchingColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, colliderLayer);
        foreach (Collider2D collidObject in touchingColliders)
        {
            if (collidObject.gameObject != this.gameObject)
            {
                Vector2 pushDirect = ((Vector2)transform.position - (Vector2)collidObject.transform.position).normalized;
                transform.position += (Vector3)pushDirect * baseStats.chargeSpeed * Time.deltaTime;
            }
        }
    }

    //Show hero collision sphere for overlap
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
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

    IEnumerator BounceAttack()
    {
        Debug.Log("<color=cyan><b>[STATE]</b> Starting Bounce Attack</color>");
        isJumping = true;
        timeCooldown = baseStats.attackCooldown;

        // 1. Visual Trigger
        animator.SetTrigger("Attack");

        // 2. Get Direction
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // 3. The "Jump" (A physical dash)
        // Use Impulse to give it immediate speed
        rb.AddForce(direction * 3f, ForceMode2D.Impulse);

        // 4. Wait for the duration of the dash
        yield return new WaitForSeconds(0.4f);

        // 5. Stop the slime (optional, or let Drag handle it)
        rb.linearVelocity = Vector2.zero;

        // Deal damage if he landed near the hero
        if (Vector2.Distance(transform.position, target.transform.position) <= 1f)
        {
            Debug.Log("<color=lime><b>[HIT]</b> Damage dealt to Hero!</color>");
            if (target.TryGetComponent(out IDamageable hitTarget))
            {
                hitTarget.takeDamage(power, transform.position, baseStats.kockbackForce);
            }
        }

        atTarget = true;
        isJumping = false;
    }
}
