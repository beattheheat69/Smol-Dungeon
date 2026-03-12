using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
/* AI for the base Hero: 
 * when enters a room, hero finds target and attack it
 * Attack has a hit box zone
 * When targets dies find new target
 * When no more monster in the room start moving to next room
 * All while not overlaping and avoiding traps
 */
public class HeroAI : Hero, IDamageable
{
    [SerializeField]
    LayerMask monsterLayer; //Layer Mask for monsters
    [SerializeField]
    LayerMask colliderLayer; //Layer for object not to overlaps with
    [SerializeField]
    LayerMask obsticleLayer; //Layer for obsicale to avoid
    GameObject target = null; // Target enemy will charge
    Rigidbody2D rb;  //Object rigidbody
    CameraManagement cameraStat; // check the stat of the camera
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // hero in attack mode
    Vector2 lastMoveDirection; // Direction the hero is looking for the attack
    Vector2 lastAngle; // Angle for the attack hitbox
    float castDistance = 0.5f; //Distance of sphere cast goes


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = baseStats.health;
        power = baseStats.power;
        cameraStat = Camera.main.GetComponent<CameraManagement>();
        //Add hero to the party if party existes
        if (HeroParty.Instance != null)
        {
            HeroParty.Instance.RegisterHeroAI(this.gameObject);
        }
    }

    //Check for target, attack if possible, if no target is found, room is done tell party the room is finished
    private void FixedUpdate()
    {
        //If camera is moving do nothing
        if (cameraStat.GetTransitionning()) return;
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
            }

            //Move to target
            MoveHero();

            //Check if can attack
            if (attacking && timeCooldown <= 0)
            {
                 DoAttack();

                //If target dead, find new one
                if (!CheckTargetAlive())
                {
                    FindTarget();
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
        if (randVal <= baseStats.attackChance)
        {
            //Hit all monster in range
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(CheckLastDirection(), lastAngle, monsterLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                IDamageable hitTarget = target.GetComponent<IDamageable>();
                hitTarget.takeDamage(power);
            }
        }
        //Start cooldown
        timeCooldown = baseStats.attackCooldown;
    }

    //Check direction of hero movement 
    Vector3 CheckLastDirection()
    {
        Vector3 direction = new Vector3();
        if (lastMoveDirection == null) return direction;
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
        //Gizmos.DrawWireCube(CheckLastDirection(), lastAngle);
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
        //caculate distance between hero and target with consistent speed
        Vector2 direction = (target.transform.position - transform.position).normalized;
        
        //Chnage direction to avoid trap if needed
        lastMoveDirection = AvoidObstical(direction);

        // Move hero toward target
        rb.MovePosition(rb.position + lastMoveDirection * baseStats.chargeSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) < 0.7f)
        {
            Vector2 pushDirect = ((Vector2)transform.position - (Vector2)target.transform.position).normalized;
            transform.position += (Vector3)pushDirect * baseStats.chargeSpeed * Time.deltaTime;
        }
        Correctoverlap();
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

    Vector2 AvoidObstical(Vector2 moveDirection)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, moveDirection, castDistance, obsticleLayer);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            Vector2 left = Vector2.Perpendicular(moveDirection).normalized;
            Vector2 right = -left;
            bool leftClear = !Physics2D.CircleCast(transform.position, 0.5f, left, 0.4f, obsticleLayer);
            bool rightClear = !Physics2D.CircleCast(transform.position, 0.5f, right, 0.4f, obsticleLayer); ;

            if (leftClear)
                return Vector2.Lerp(moveDirection, left, 0.9f).normalized;
            else if (rightClear)
                return Vector2.Lerp(moveDirection, right, 0.9f).normalized;
            else
                return -moveDirection; // dead end → back up
        }
        else 
        {
            return moveDirection;
        }
    }

    //Find which monster is the closest
    private void FindTarget()
    {
        float closestDist = Mathf.Infinity; //smallest distance between hero and monsters
        GameObject nearTarget = null; //closest monster to hero
        List<GameObject> monsters = HeroParty.Instance.GetRoom().GetList(); //Get List of monsters in the room

        //Check all monsters to see which is closest
        foreach (GameObject monster in monsters)
        {
            //Check if monster is active
            if (monster.activeSelf)
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
        target = nearTarget;
    }

    //Move hero toward the exit door to go to next room
    public bool MoveToDoor(Vector3 position)
    {
        Vector2 direction = (position - transform.position).normalized;
        direction = AvoidObstical(direction);
        rb.MovePosition(rb.position + direction * baseStats.chargeSpeed * Time.fixedDeltaTime);
        Correctoverlap();
        //Check if hero still needs to move
        if (Vector2.Distance(transform.position, position) > 0.05f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
