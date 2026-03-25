using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Tilemaps;
using UnityEngine;
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
    public Vector2 lastMoveDirection; // Direction the hero is looking for the attack
    Vector2 lastAngle; // Angle for the attack hitbox
    float castDistance = 0.8f; //Distance of sphere cast goes
    float avoidWeight = 2.5f;
    BoxCollider2D boxCol;
    private float sideChoiceTimer = 0f;
    private int sideChoice = 0; // -1 = left, 1 = right, 0 = none
    HeroAnimation heroAnim;


    private void Start()
    {
        heroAnim = GetComponent<HeroAnimation>();
        rb = GetComponent<Rigidbody2D>();
        cameraStat = Camera.main.GetComponent<CameraManagement>();
        boxCol = GetComponent<BoxCollider2D>();
        //Add hero to the party if party existes
        if (HeroParty.Instance != null)
        {
            HeroParty.Instance.RegisterHeroAI(this.gameObject);
        }
        if (HeroDataManager.Instance != null)
        {
            base.index = HeroDataManager.Instance.party.Count;
            HeroDataManager.Instance.party.Add(new HeroData { currentHealt = baseStats.health, dodgeChance = baseStats.dodgeChange });
            health = HeroDataManager.Instance.party[index].currentHealt;  // testing hero health
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

            if (!atTarget)
            {
                //Move to target
                MoveHero();
            }


            //Check if can attack
            if (attacking && timeCooldown <= 0)
            {
                DoAttack();
                heroAnim.IsAttacking();
            }

            //If target dead, find new one
            if (!CheckTargetAlive())
            {
                FindTarget();
            }

            //attack cooldown
            if (timeCooldown > 0)
            {
                timeCooldown -= Time.deltaTime;
            }
        }
    }

    //Enter in attack mode when colliding with target
    /*  private void OnTriggerEnter2D(Collider2D collision)
      {
          if (collision.gameObject == target && !attacking)
          {
              attacking = true;
          }
          else if (collision.gameObject != target && !attacking && collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
          {
              target = collision.gameObject;
              Debug.Log(collision.gameObject.name);
          }
      }

      //Leaves attack mode when not colliding with target
      private void OnTriggerExit2D(Collider2D collision)
      {
          if (collision.gameObject.gameObject == target)
          {
              attacking = false;
          }
      }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("<color=DeepPink><b>[STATE]</b> Starting Bounce Attack</color>");
        if (collision.gameObject == target && !attacking)
        {
            attacking = true;
        }
        else if (collision.gameObject != target && !attacking && collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            target = collision.gameObject;
            Debug.Log(collision.gameObject.name);
        }
    }

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
        return target.GetComponent<Character>().IsAlive();
    }

    //Does an attack with a range infront of him, according to his direction of movement
    private void DoAttack()
    {
        float randVal = Random.Range(1, 100);

        //Check is attack succeded
        if (randVal <= baseStats.attackChance)
        {
            //Hit all monster in range
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(CheckLastDirection(), lastAngle, 0f, monsterLayer); //Check arguments, layer in place of angle
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out IDamageable hitTarget)) //BUG: One shots monster
                {
                    Debug.Log("<color=blue><b>[HIT]</b> Damage dealt by Hero!</color>");
                    hitTarget.takeDamage(baseStats.power, transform.position, baseStats.kockbackForce);  // add buff or debuff
                }
            }
        }
        //Start cooldown
        timeCooldown = baseStats.attackCooldown;
    }

    //Check direction of hero movement 
    public Vector3 CheckLastDirection()
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
        //caculate distance between hero and target with consistent speed
        Vector2 seek = ((Vector2)target.transform.position - rb.position).normalized;
        Vector2 avoid = AvoidObstical(seek);

        //Chnage direction to avoid trap if needed
        lastMoveDirection = (seek + avoid * avoidWeight).normalized;

        // Move hero toward target
        rb.MovePosition(rb.position + lastMoveDirection * baseStats.chargeSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) < 1f)
        {
          atTarget = true;
        }
        //Correctoverlap();
    }

  /*  private void Correctoverlap()
    {
        Collider2D[] touchingColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, colliderLayer);
        foreach (Collider2D collidObject in touchingColliders)
        {
            if (collidObject.gameObject != this.gameObject)
            {
                Vector2 pushDirect = ((Vector2)transform.position - (Vector2)collidObject.transform.position).normalized;
                rb.MovePosition(rb.position + pushDirect * baseStats.chargeSpeed * Time.fixedDeltaTime);
            }
        }
    }*/

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

        // If forward is clear → no avoidance needed
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
        float closestDist = Mathf.Infinity; //smallest distance between hero and monsters
        GameObject nearTarget = null; //closest monster to hero
        List<GameObject> monsters = HeroParty.Instance.GetRoom().GetList(); //Get List of monsters in the room

        //***Add check if no monster list available? Pour l'entrance, pas de monstres alors hero va direct a ExitDoor

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
        atTarget = false;
        target = nearTarget;
    }

    //Move hero toward the exit door to go to next room
    public bool MoveToDoor(Vector2 position, bool atdoor)
    {
        // Convert target to 2D
        Vector2 target = new Vector2(position.x, position.y);

        //Find distance between target and hero
        Vector2 seek = ((Vector2)target - rb.position).normalized;
        //Find direction to avoid object
        Vector2 avoid = AvoidObstical(seek);

        lastMoveDirection = (seek + avoid * avoidWeight).normalized;


        rb.MovePosition(rb.position + lastMoveDirection * baseStats.chargeSpeed * Time.fixedDeltaTime);


        //Correctoverlap();

        // Arrival check using rb.position and a realistic threshold
        return Vector2.Distance(rb.position, target) <= 0.05f;

    }

    public void ChangeColliderTrigger(bool atdoor)
    {
        transform.GetComponent<BoxCollider2D>().isTrigger = atdoor;
    }

}
