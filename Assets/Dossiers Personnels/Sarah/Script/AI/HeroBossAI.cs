using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class HeroBossAI : Hero
{
    [SerializeField]
    LayerMask colliderLayer; //Layer for object not to overlaps with
    [SerializeField]
    LayerMask obsticleLayer; //Layer for obsicale to avoid
    GameObject target = null; // Target enemy will charge
    //Rigidbody2D rb;  //Object rigidbody
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // hero in attack mode
    Vector2 lastAngle; // Angle for the attack hitbox
    float castDistance = 0.8f; //Distance of sphere cast goes
    float avoidWeight = 2.5f;
    BoxCollider2D boxCol;
    private float sideChoiceTimer = 0f;
    private int sideChoice = 0; // -1 = left, 1 = right, 0 = none
    HeroAnimation heroAnim;
    [SerializeField] Lifebar lifebar; //J'ai aussi ajouté une ligne de code dans Start et dans Update


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        heroAnim = GetComponent<HeroAnimation>();
        boxCol = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        FindTarget();
        health = HeroDataManager.Instance.party[index].currentHealt;
		lifebar.SetMaxHealth(baseStats.health);
	}

    // Update is called once per frame
    void Update()
    {
		lifebar.SetHealth(health);

        if (target != null && !atTarget)
        {
            MoveHero();
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

        if (attacking && timeCooldown <= 0)
        {
            DoAttack();
            heroAnim.IsAttacking();
        }

		//Cooldown timer
		if (timeCooldown >= 0)
			timeCooldown -= Time.deltaTime;
	}

    //Enter in attack mode when colliding with target
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target && !attacking)
        {
            //Debug.Log(Vector2.Distance(transform.position, target.transform.position));
            attacking = true;
            atTarget = true;
            rb.linearVelocity = Vector2.zero;
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

    //Move Hero towards target
    private void MoveHero()
    {
        //caculate distance between hero and target with consistent speed
        Vector2 closestPoint = target.GetComponent<Collider2D>().ClosestPoint(rb.position);

        float heroRadius = boxCol.bounds.extents.x;
        float distanceToSurface = Vector2.Distance(rb.position, closestPoint);
        float stopDistance = heroRadius + 0.05f;

        if (distanceToSurface <= stopDistance)
        {
            atTarget = true;
            attacking = true;
            rb.linearVelocity = Vector2.zero; // Kill any sliding momentum
            return;
        }

        //Change direction to avoid trap if needed
        lastMoveDirection = (((Vector2)target.transform.position - (Vector2)rb.position) + AvoidObstical(closestPoint)).normalized;

        // Compute movement step
        float moveStep = baseStats.chargeSpeed * Time.fixedDeltaTime;

        // Clamp movement so we never cross the boundary
        moveStep = Mathf.Min(moveStep, distanceToSurface - stopDistance);

        // Move hero
        Vector2 nextPos = rb.position + lastMoveDirection * moveStep;
        rb.MovePosition(nextPos);
    }

    private void DoAttack()
    {
        float randVal = Random.Range(1, 100);
        
        //Check is attack succeded
        if (randVal <= baseStats.attackChance)
        {   
            IDamageable hitTarget = target.GetComponent<IDamageable>();
            hitTarget.takeDamage(baseStats.power, transform.position, 0f);  // add buff or debuff
        }
        //Start cooldown
        timeCooldown = baseStats.attackCooldown;
    }

    Vector2 AvoidObstical(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return Vector2.zero;

        // World collider size
        Vector2 worldSize = new Vector2(
            boxCol.size.x * Mathf.Abs(transform.lossyScale.x),
            boxCol.size.y * Mathf.Abs(transform.lossyScale.y)
        );

        Vector2 worldCenter = rb.position + boxCol.offset * transform.lossyScale;

        // Directions
        Vector2 left = Vector2.Perpendicular(direction);
        Vector2 right = -left;

        // Casts
        RaycastHit2D hitForward = Physics2D.BoxCast(worldCenter, worldSize, 0f, direction, castDistance, obsticleLayer);
        RaycastHit2D hitLeft = Physics2D.BoxCast(worldCenter, worldSize, 0f, left, castDistance, obsticleLayer);
        RaycastHit2D hitRight = Physics2D.BoxCast(worldCenter, worldSize, 0f, right, castDistance, obsticleLayer);

        // If forward is clear ? no avoidance needed
        if (hitForward.collider == null)
        {
            sideChoice = 0;
            return Vector2.zero;
        }

        // If we already chose a side, stick to it for a moment
        if (sideChoiceTimer > 0f)
        {
            sideChoiceTimer -= Time.fixedDeltaTime;
            return sideChoice == -1 ? left : right;
        }

        // Choose the best side
        float leftDist = hitLeft.collider == null ? castDistance : hitLeft.distance;
        float rightDist = hitRight.collider == null ? castDistance : hitRight.distance;

        if (leftDist > rightDist)
            sideChoice = -1; // go left
        else
            sideChoice = 1;  // go right

        sideChoiceTimer = 0.3f; // commit for 0.3 seconds

        return sideChoice == -1 ? left : right;
    }


    //Find which monster is the closest
    private void FindTarget()
    {
        target = GameObject.Find("Boss");
    }
}
