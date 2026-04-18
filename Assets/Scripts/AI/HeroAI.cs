using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;
/* AI for the base Hero: 
 * when enters a room, hero finds target and attack it
 * Attack has a hit box zone
 * When targets dies find new target
 * When no more monster in the room start moving to next room
 * All while not overlaping and avoiding traps
 */
public class HeroAI : Hero
{
    [SerializeField]
    LayerMask monsterLayer; //Layer Mask for monsters
    [SerializeField]
    LayerMask colliderLayer; //Layer for object not to overlaps with
    [SerializeField]
    LayerMask obsticleLayer; //Layer for obsicale to avoid
    GameObject target = null; // Target enemy will charge
    //Rigidbody2D rb;  //Object rigidbody
    CameraManagement cameraStat; // check the stat of the camera
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // hero in attack mode
    Vector2 lastAngle; // Angle for the attack hitbox
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


    private void Start()
    {
        heroAnim = GetComponent<HeroAnimation>();
        rb = GetComponent<Rigidbody2D>();
        cameraStat = Camera.main.GetComponent<CameraManagement>();
        boxCol = GetComponent<BoxCollider2D>();
        lifebar.SetMaxHealth(baseStats.health);
        //Add hero to the party if party existes
        if (HeroParty.Instance != null)
        {
            HeroParty.Instance.RegisterHeroAI(this.gameObject);
        }
        if (HeroDataManager.Instance != null)
        {
            HeroDataManager.Instance.party[index] = new HeroData { currentHealt = baseStats.health, dodgeChance = baseStats.dodgeChange };
            health = HeroDataManager.Instance.party[index].currentHealt;  // testing hero health
        }
    }

    //Check for target, attack if possible, if no target is found, room is done tell party the room is finished
    private void FixedUpdate()
    {
        lifebar.SetHealth(health);

        //If camera is moving do nothing
        if (cameraStat.GetTransitionning() || isStunned) return;

        if (!HeroParty.Instance.GetRoomFinised())
        {
            //If no target check for one
            if (target == null)
            {
                FindTarget();
                // If still no target room is finished, jump to next update
                if (target == null)
                {
                    HeroParty.Instance.SetRoomFinised(true);
                    return;
                }
                else
                {
                    lastMoveDirection = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;
                }
            }

            if (!atTarget)
            {
                //Move to target
                MoveHero();
            }
            else if (byCheck)
            {
                CheckCollider();
            }


            //Check if can attack
            if (attacking && timeCooldown <= 0)
            {
                DoAttack();
                heroAnim.IsAttacking(); //Bug? Call here when walking towards next target

                //If target dead, find new one
                if (!CheckTargetAlive())
                {
                    FindTarget();
                }

            }

            //attack cooldown
            if (timeCooldown >= 0)
            {
                timeCooldown -= Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target)
        {
            lastMoveDirection = ((Vector2)target.transform.position - (Vector2)rb.position).normalized;
            attacking = true;
            atTarget = true;
            rb.linearVelocity = Vector2.zero; // Kill any sliding momentum
        }
        else if (!attacking && collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            float currentTargetDistance = Vector2.Distance(rb.position, target.transform.position);
            float newTargetDistance = Vector2.Distance(rb.position, collision.transform.position);

            float stickRadius = 1.5f;

            // If hero is still close to current target, don't switch
            if (currentTargetDistance <= stickRadius)
                return;

            // Otherwise switch only if new target is closer
            if (newTargetDistance < currentTargetDistance)
            {
                target = collision.gameObject;

                //change 
            }
        }
    }

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
        return target.GetComponent<Character>().IsAlive();
    }

    //Does an attack with a range infront of him, according to his direction of movement
    private void DoAttack()
    {
        float randVal = Random.Range(1, 100);

        rb.linearVelocity = Vector2.zero;
        //Check is attack succeded
        if (randVal <= baseStats.attackChance)
        {
            //Hit all monster in range
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(CheckLastDirection(), lastAngle, 0f, monsterLayer); //Check arguments, layer in place of angle
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out IDamageable hitTarget)) //BUG: One shots monster
                {
                    hitTarget.takeDamage(baseStats.power, transform.position, baseStats.kockbackForce);  // add buff or debuff
                    //atTarget = false;
                }
            }
        }
        else
        {
            //Calls miss anim and text on enemy
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(CheckLastDirection(), lastAngle, 0f, monsterLayer); //Check arguments, layer in place of angle
            foreach (Collider2D enemy in hitEnemies)
            {
                damageNumberAnim.GetComponentInChildren<TextMesh>().text = "Miss";
                damageNumberAnim.GetComponentInChildren<TextMesh>().color = UnityEngine.Color.white;
                GameObject inst = Instantiate(damageNumberAnim, enemy.transform.position, Quaternion.identity);
                Destroy(inst, 1f);
            }
        }
        //Start cooldown
        timeCooldown = baseStats.attackCooldown;
    }

    //Check direction of hero movement 
    public Vector3 CheckLastDirection()
    {
        Vector3 direction = new Vector3();
        if (lastMoveDirection == null)
        {
            return direction;
        }
        if (lastMoveDirection.y > 0.05f) // going upwards
        {
            direction = transform.position + new Vector3(0, 0.5f, 0f);
            lastAngle = new Vector2(1.3f, 0.6f);
        }
        else if (lastMoveDirection.y < -0.05f) // going downwards
        {
            direction = transform.position + new Vector3(0, -0.5f, 0f);
            lastAngle = new Vector2(1.3f, 0.6f);
        }
        else if (lastMoveDirection.x > 0.05f) // going right
        {
            direction = transform.position + new Vector3(0.5f, 0f, 0f);
            lastAngle = new Vector2(0.6f, 1.3f);
        }
        else if (lastMoveDirection.x < -0.05f) // going left
        {
            direction = transform.position + new Vector3(-0.5f, 0f, 0f);
            lastAngle = new Vector2(0.6f, 1.3f);
        }
        return direction;
    }

    //To visualize casts
    void OnDrawGizmosSelected()
    {
        //Show hero hitbox depending og his moving direction
        Gizmos.DrawWireCube(CheckLastDirection(), lastAngle);
        //Show hero collision sphere for overlap
        /*Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);*/
        //Show hero hitboxes for all four direction box
        /*Gizmos.DrawWireCube(transform.position + new Vector3(0f, 0.5f, 0f), new Vector2(1.3f, 0.6f)); //haut
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, -0.5f, 0f), new Vector2(1.3f, 0.6f)); //bas
        Gizmos.DrawWireCube(transform.position + new Vector3(0.5f, 0f, 0f), new Vector2(0.6f, 1.3f)); //droite
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.5f, 0f, 0f), new Vector2(0.6f, 1.3f)); //gauche*/
        //Show avoidance shpere direction
        /*Gizmos.color = UnityEngine.Color.purple;
        Gizmos.DrawWireSphere(transform.position + (Vector3)lastMoveDirection * castDistance, 0.6f);*/
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

    void CheckCollider()
    {
        Vector2 closestPoint = target.GetComponent<Collider2D>().ClosestPoint(rb.position);

        float heroRadius = boxCol.bounds.extents.x;
        float distanceToSurface = Vector2.Distance(rb.position, closestPoint);
        float stopDistance = heroRadius + 0.06f;


        if ((distanceToSurface > stopDistance) && !(Mathf.Approximately(distanceToSurface, stopDistance)))
        {
            atTarget = false;
            attacking = false;
            byCheck = false;
        }
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

    //Find which monster is the closest
    private void FindTarget()
    {
        float closestDist = Mathf.Infinity; //smallest distance between hero and monsters
        GameObject nearTarget = null; //closest monster to hero
        List<GameObject> monsters = HeroParty.Instance.GetRoom().GetList(); //Get List of monsters in the room

        //***Add check if no monster list available? Pour l'entrance, pas de monstres alors hero va direct a ExitDoor

        //Check all monsters to see which is closest
        foreach (GameObject monster in monsters)
        {
            //Check if monster is active
            if (monster.activeSelf && monster.GetComponent<Character>().IsAlive())
            {
                float distance = Vector2.Distance(transform.position, monster.transform.position);
                if (distance < closestDist)
                {
                    closestDist = distance;
                    nearTarget = monster;
                }
            }
        }
        //Set the monster that is closest hero
        atTarget = false;
        attacking = false;
        target = nearTarget;
    }

    //Move hero toward the exit door to go to next room
    public bool MoveToDoor(Vector2 position, bool atdoor)
    {
        Vector2 toTarget = (position - rb.position).normalized;

        Vector2 finalDir = ComputeSteering(toTarget);

        rb.MovePosition(rb.position + finalDir * baseStats.chargeSpeed * Time.fixedDeltaTime);

        return Vector2.Distance(rb.position, position) <= 0.05f;

    }

    public void ChangeColliderTrigger(bool atdoor)
    {
        transform.GetComponent<BoxCollider2D>().isTrigger = atdoor;
    }

}
