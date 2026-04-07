using UnityEditor.Tilemaps;
using UnityEngine;

public class ArmorAI : MonsterAI
{
    public Armor_Stats_SO baseStats; // Base stats of slime
    [SerializeField]
    LayerMask colliderLayer;
    CameraManagement cameraStat; // check the stat of the camera
    GameObject target = null; // current hero target
    //Rigidbody2D rb;  //Object rigidbody
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // monster in attack mode
    bool isActive = false;
    BoxCollider2D boxCol;
    HeroAnimation charAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charAnim = GetComponent<HeroAnimation>();
        rb = GetComponent<Rigidbody2D>();
        health = baseStats.health;
        power = baseStats.power;
        cameraStat = Camera.main.GetComponent<CameraManagement>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (cameraStat.GetTransitionning()) return;
            if (isActive)
            {
                if (target == null)
                {
                    FindTarget();
                }

                if (!atTarget)
                {
                    //Move to target
                    MoveEnemy();
                }
                else
                {
                    // Recompute distance to target
                    Vector2 closestPoint = target.GetComponent<Collider2D>().ClosestPoint(rb.position);

                    float heroRadius = boxCol.bounds.extents.x;
                    float distanceToSurface = Vector2.Distance(rb.position, closestPoint);
                    float stopDistance = heroRadius + 0.05f;

                    // If hero is no longer close enough, resume movement
                    if (distanceToSurface > stopDistance + 0.1f) // small buffer
                    {
                        atTarget = false;
                        attacking = false;
                    }
                }

                //Check if can attack
                if (attacking && timeCooldown <= 0)
                {
                    DoAttack();
                    charAnim.IsAttacking(); ;

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
       
    }


   /* bool CheckIfAtTarget()
    {
        Vector2 closestPoint = target.GetComponent<Collider2D>().ClosestPoint(rb.position);

        float radius = boxCol.bounds.extents.x;
        float distanceToSurface = Vector2.Distance(rb.position, closestPoint);
        float stopDistance = radius + 0.05f;

        if (distanceToSurface <= stopDistance)
        {
            atTarget = true;
            attacking = true;
            rb.linearVelocity = Vector2.zero;
            return true;
        }

        atTarget = false;
        attacking = false;
        return false;
    }*/

    //Enter in attack mode when colliding with target
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive && collision.gameObject == target)
        {
            if (collision.gameObject == target && !attacking)
            {
                lastMoveDirection = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;
            }
        }
        else if (collision.transform.tag == "Hero") 
        {
            ActivateArmor();    
            target = collision.gameObject;
        }

    }

    public void ActivateArmor()
    {
        transform.GetComponent<CapsuleCollider2D>().enabled = false;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        FindTarget();
        gameObject.layer = LayerMask.NameToLayer("Monster");
        isActive = true;
    }

    //Leaves attack mode when not colliding with target
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.gameObject == target)
        {
            attacking = false;
            atTarget = false;
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
        Debug.Log("Armor attacked");
        float randVal = Random.Range(1, 100);
        GetComponent<SoundCaster>().PlayAttackSFX();

        //Check is attack succeded
        if (randVal <= baseStats.attackChance)
        {
            rb.linearVelocity = Vector2.zero;
            if (target.TryGetComponent(out IDamageable hitTarget)) //BUG: One shots hero
            {
                hitTarget.takeDamage(power, transform.position, baseStats.kockbackForce);
                //GetComponent<Animator>().SetTrigger("Attack");
                atTarget = false;
            }
        }
        //Start cooldown
        timeCooldown = baseStats.attackCooldown;
    }

    //Move monster towards target
    private void MoveEnemy()
    {
        //caculate distance between hero and target with consistent speed
        Vector2 closestPoint = target.GetComponent<Collider2D>().ClosestPoint(rb.position);

        float armorRadius = boxCol.bounds.extents.x;
        float heroRadius = target.GetComponent<Collider2D>().bounds.extents.x;
        float stopDistance = armorRadius + heroRadius + 0.05f;
        float distanceToSurface = Vector2.Distance(rb.position, closestPoint);
        Debug.Log("Armor distanceToSurface = " + distanceToSurface);
        if (distanceToSurface <= stopDistance)
        {
            atTarget = true;
            attacking = true;
            rb.linearVelocity = Vector2.zero; // Kill any sliding momentum
            return;
        }

        // Calculate direction from physics position
        lastMoveDirection = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;

        // Compute movement step
        float moveStep = baseStats.chargeSpeed * Time.fixedDeltaTime;

        // Clamp movement so we never cross the boundary
        moveStep = Mathf.Max(0f, Mathf.Min(moveStep, distanceToSurface - stopDistance));

        // Move hero
        Vector2 nextPos = rb.position + lastMoveDirection * moveStep;
        rb.MovePosition(nextPos);

        // Move hero toward target
        //rb.MovePosition(rb.position + lastMoveDirection * baseStats.chargeSpeed * Time.fixedDeltaTime);
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

    public Vector2 GetDirection()
    {
        return lastMoveDirection;
    }

}
