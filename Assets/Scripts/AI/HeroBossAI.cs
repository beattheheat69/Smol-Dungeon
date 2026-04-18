using System.Collections.Generic;
using Unity.VisualScripting;
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
    float castDistance = 0.8f; //Distance of sphere cast goes
    BoxCollider2D boxCol;
    HeroAnimation heroAnim;
    bool byCheck = false;
    float stuckTimer = 0f;
    [SerializeField] Lifebar lifebar; //J'ai aussi ajouté une ligne de code dans Start et dans Update

    struct AvoidanceResult
    {
        public Vector2 direction; // left or right
        public float distance;    // distance to obstacle ahead
    }


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
        else if (byCheck)
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
                byCheck = false;
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
            lastMoveDirection = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;
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
        Collider2D col = target.GetComponent<Collider2D>();
        float heroRadius = boxCol.bounds.extents.x;

        float moveStep = baseStats.chargeSpeed * Time.fixedDeltaTime;

        if (col != null)
        {
            Vector2 closestPoint = col.ClosestPoint(rb.position);
            float distanceToSurface = Vector2.Distance(rb.position, closestPoint);
            float stopDistance = heroRadius + 0.05f;

            if (distanceToSurface <= stopDistance)
            {
                atTarget = true;
                attacking = true;
                byCheck = true;
                rb.linearVelocity = Vector2.zero;
                return;
            }

            moveStep = Mathf.Min(moveStep, distanceToSurface - stopDistance);
        }

        Vector2 toTarget = ((Vector2)target.transform.position - rb.position).normalized;
        Vector2 finalDir = ComputeSteering(toTarget);

        rb.MovePosition(rb.position + finalDir * moveStep);
    }


    AvoidanceResult AvoidObstical(Vector2 desiredDir)
    {
        AvoidanceResult result = new AvoidanceResult();

        if (desiredDir == Vector2.zero)
            return result;

        Vector2 worldSize = new Vector2(
            boxCol.size.x * Mathf.Abs(transform.lossyScale.x),
            boxCol.size.y * Mathf.Abs(transform.lossyScale.y)
        );

        Vector2 worldCenter = rb.position + boxCol.offset * transform.lossyScale;

        Vector2 left = Vector2.Perpendicular(desiredDir);
        Vector2 right = -left;

        // Forward cast
        RaycastHit2D hitForward = Physics2D.BoxCast(
            worldCenter, worldSize, 0f,
            desiredDir, castDistance, obsticleLayer
        );

        if (hitForward.collider == null)
            return result;

        // 1. Scan left until clear
        float leftScan = 0f;
        bool leftClear = false;

        while (leftScan < 2f)
        {
            Vector2 offset = left * leftScan;

            RaycastHit2D scan = Physics2D.BoxCast(
                worldCenter + offset, worldSize, 0f,
                desiredDir, castDistance, obsticleLayer
            );

            if (scan.collider == null)
            {
                leftClear = true;
                break;
            }

            leftScan += 0.2f;
        }

        // 2. Scan right until clear
        float rightScan = 0f;
        bool rightClear = false;

        while (rightScan < 2f)
        {
            Vector2 offset = right * rightScan;

            RaycastHit2D scan = Physics2D.BoxCast(
                worldCenter + offset, worldSize, 0f,
                desiredDir, castDistance, obsticleLayer
            );

            if (scan.collider == null)
            {
                rightClear = true;
                break;
            }

            rightScan += 0.2f;
        }


        // 3. Choose the correct side
        if (leftClear && rightClear)
        {
            // Both sides open → choose the side that bends toward the target
            float leftAngle = Vector2.Angle(desiredDir, left);
            float rightAngle = Vector2.Angle(desiredDir, right);

            result.direction = (leftAngle < rightAngle ? left : right);
        }
        else if (leftClear)
        {
            result.direction = left;
        }
        else if (rightClear)
        {
            result.direction = right;
        }
        else
        {
            // Both sides blocked → fallback to left
            result.direction = left;
        }

        result.distance = hitForward.distance;
        return result;
    }

    private Vector2 ComputeSteering(Vector2 toTarget)
    {
        // 0. Fallback: if toTarget is zero, use lastMoveDirection or default
        if (toTarget == Vector2.zero)
        {
            if (lastMoveDirection != Vector2.zero)
                toTarget = lastMoveDirection;
            else
                toTarget = Vector2.right; // default fallback
        }

        // 1. Ask avoidance for help
        AvoidanceResult avoid = AvoidObstical(toTarget);

        float heroRadius = boxCol.bounds.extents.x;

        // 2. Spike fix: safe distance based on trap size (2.59 x 2.5)
        float trapHalfSize = 1.295f;
        float safeDistance = heroRadius + trapHalfSize + 0.2f;

        // 3. Distance-weighted avoidance
        float avoidanceWeight = 0f;
        if (avoid.direction != Vector2.zero)
            avoidanceWeight = Mathf.Clamp01(1f - (avoid.distance / safeDistance));

        // 4. Clearance bubble
        bool tooClose = Physics2D.CircleCast(
            rb.position,
            heroRadius + 0.15f,
            Vector2.zero,
            0f,
            obsticleLayer
        );

        // 5. Combine directions
        Vector2 finalDir = toTarget;

        if (avoid.direction != Vector2.zero)
            finalDir = (toTarget + avoid.direction * avoidanceWeight).normalized;

        if (tooClose && avoid.direction != Vector2.zero)
            finalDir = avoid.direction;

        // 6. Crossbow fix: stuck timer
        if (rb.linearVelocity.magnitude < 0.05f)
            stuckTimer += Time.fixedDeltaTime;
        else
            stuckTimer = 0f;

        if (stuckTimer > 0.2f && avoid.direction != Vector2.zero)
            finalDir = avoid.direction;

        if (finalDir.magnitude < 0.1f)
            finalDir = Vector2.zero;

        // 7. Update lastMoveDirection
        if (finalDir != Vector2.zero)
        {
            lastMoveDirection = Vector2.Lerp(
                lastMoveDirection,
                finalDir,
                Time.fixedDeltaTime * 10f   // smoothing speed
            );
        }

        return finalDir;
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
		else
		{
			//Calls miss anim and text on enemy
			damageNumberAnim.GetComponentInChildren<TextMesh>().text = "Miss";
			damageNumberAnim.GetComponentInChildren<TextMesh>().color = Color.white;
			GameObject inst = Instantiate(damageNumberAnim, target.transform.position, Quaternion.identity);
			Destroy(inst, 1f);
		}
		//Start cooldown
		timeCooldown = baseStats.attackCooldown;
    }

    //Find which monster is the closest
    private void FindTarget()
    {
        target = GameObject.Find("Boss");
    }
}
