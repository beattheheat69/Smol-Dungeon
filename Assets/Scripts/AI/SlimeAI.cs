using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SlimeAI : MonsterAI
{
    public SlimeStats_SO baseStats; // Base stats of slimes
    [SerializeField]
    LayerMask colliderLayer;
    CameraManagement cameraStat; // check the stat of the camera
    GameObject target = null; // current hero target
    //Rigidbody2D rb;  //Object rigidbody
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // monster in attack mode
    bool isJumping = false; //Is doinf the bounce attack
    CircleCollider2D circleCol;
    bool byCheck = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = baseStats.health;
        power = baseStats.power;
        cameraStat = Camera.main.GetComponent<CameraManagement>();
        circleCol = GetComponent<CircleCollider2D>();
    }

    //Check for target, attack if possible
    private void FixedUpdate()
    {
        if (!isDead)
        {
            //If camera is moving do nothing
            if (cameraStat.GetTransitionning() || isStunned) return;
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
            else if (byCheck)
            {
                CheckCollider();
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

        if (lastMoveDirection.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
			GetComponent<SpriteRenderer>().flipX = false;
	}

    //Enter in attack mode when colliding with target
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target && !attacking)
        {
            attacking = true;
            atTarget = true;
            rb.linearVelocity = Vector2.zero;
            lastMoveDirection = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;
        }
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

    //Does an attack on target
    private void DoAttack()
    {
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(BounceAttack());
        /* if (target.TryGetComponent(out IDamageable hitTarget)) //BUG: One shots hero
         {
             hitTarget.takeDamage(baseStats.power, transform.position, 0f);
             animator.SetTrigger("Attack");
         }

         //Start cooldown
         timeCooldown = baseStats.attackCooldown;*/
    }

    //Move monster towards target
    private void MoveEnemy()
    {
        if (isJumping || atTarget) return;

        Vector2 closestPoint = target.GetComponent<Collider2D>().ClosestPoint(rb.position);

        float slimeRadius = circleCol.radius * Mathf.Abs(transform.lossyScale.x);

        float distanceToSurface = Vector2.Distance(rb.position, closestPoint);
        float stopDistance = slimeRadius + 0.05f;

        if (distanceToSurface <= stopDistance || Mathf.Approximately(distanceToSurface, stopDistance))
        {
            atTarget = true;
            attacking = true;
            rb.linearVelocity = Vector2.zero; // Kill any sliding momentum
            return;
        }

        lastMoveDirection = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;

        float moveStep = baseStats.chargeSpeed * Time.fixedDeltaTime;

        moveStep = Mathf.Min(moveStep, distanceToSurface - stopDistance);

        // 8. Move slime
        Vector2 nextPos = rb.position + lastMoveDirection * moveStep;
        rb.MovePosition(nextPos);

        Vector2 currentPos = rb.position;
        // 2. Exact Circle Distance Logic
        // We find the edge of the target, then subtract the slime's actual radius.
        Collider2D targetCol = target.GetComponent<Collider2D>();
        Vector2 closestPointOnTarget = targetCol.ClosestPoint(currentPos);

        float distanceToTargetEdge = Vector2.Distance(currentPos, closestPointOnTarget);
        

        // 3. Trigger the BounceAttack
        float attackRange = 3f;

        // 'distanceToTargetEdge' is the gap between slime center and target skin.
        // Subtract slimeRadius to get 'gap between skins'.
        float skinToSkinGap = distanceToTargetEdge - slimeRadius;

        if (skinToSkinGap <= attackRange && skinToSkinGap > 0.1f)
        {
            atTarget = true;
            attacking = true;
            //timeCooldown = baseStats.attackCooldown;
            //StartCoroutine(BounceAttack());
        }
    }

    void CheckCollider()
    {
        Vector2 closestPoint = target.GetComponent<Collider2D>().ClosestPoint(rb.position);

        float slimeRadius = circleCol.radius * Mathf.Abs(transform.lossyScale.x);
        float distanceToSurface = Vector2.Distance(rb.position, closestPoint);
        float stopDistance = slimeRadius + 0.06f;


        if ((distanceToSurface > stopDistance) && !(Mathf.Approximately(distanceToSurface, stopDistance)))
        {
            atTarget = false;
            attacking = false;
            byCheck = false;
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
        isJumping = true;
        timeCooldown = baseStats.attackCooldown;
        animator.SetTrigger("Attack");
        GetComponent<SoundCaster>().PlayAttackSFX();

        //Windup anim + delay
		rb.linearVelocity = Vector2.zero;
		yield return new WaitForSeconds(0.5f);

		// Get Direction
		Vector2 direction = (target.transform.position - transform.position).normalized;

        // Use Impulse to give it immediate speed
        rb.AddForce(direction * 3f, ForceMode2D.Impulse);

        // Wait for the duration of the dash
        yield return new WaitForSeconds(0.4f);

        // Stop the slime for short knockback
        rb.linearVelocity = Vector2.zero;

        // Deal damage if he landed near the hero
        if (Vector2.Distance(transform.position, target.transform.position) <= 2f)
        {
            if (target.TryGetComponent(out IDamageable hitTarget))
            {
                hitTarget.takeDamage(power, transform.position, baseStats.kockbackForce);
            }
        }

        atTarget = false;
        isJumping = false;
    }
}
