using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class HeroAI : Hero, IDamageable
{
    [SerializeField]
    LayerMask monsterLayer; //Layer Mask for monsters

    GameObject target = null; // Target enemy will charge
    Rigidbody2D rb;  //Object rigidbody
    CameraManagement camera;
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // hero in attack mode
    Vector2 lastMoveDirection; // Direction the hero is looking for the attack
    Vector2 lastAngle; // Angle for the attack hitbox


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = baseStats.health;
        power = baseStats.power;
        camera = Camera.main.GetComponent<CameraManagement>();
    }

    //Check for target, attack if possible
    private void FixedUpdate()
    {
        //If camera is moving do nothing
        if (camera.GetTransitionning()) return;
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
                MoveEnemy();

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
        if (lastMoveDirection.y > 0.05f)
        {
            direction = transform.position + new Vector3(0.01f, 0.17f, 0f);
            lastAngle = new Vector2(0.4f, 0.125f);
        }
        else if (lastMoveDirection.y < -0.05f)
        {
            direction = transform.position + new Vector3(0.01f, -0.025f, 0f);
            lastAngle = new Vector2(0.4f, 0.125f);
        }
        else if (lastMoveDirection.x > 0.05f)
        {
            direction = transform.position + new Vector3(0.08f, 0.1f, 0f);
            lastAngle = new Vector2(0.125f, 0.4f);
        }
        else if (lastMoveDirection.x < -0.05f)
        {
            direction = transform.position + new Vector3(-0.08f, 0.1f, 0f);
            lastAngle = new Vector2(0.125f, 0.4f);
        }
        return direction;
    }

    
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(CheckLastDirection(), lastAngle); //haut
        //Show hero hitboxes for all four direction
        /*Gizmos.DrawWireCube(transform.position + new Vector3(0.01f, 0.17f, 0f), new Vector2(0.4f, 0.125f)); //haut
        Gizmos.DrawWireCube(transform.position + new Vector3(0.01f, -0.025f, 0f), new Vector2(0.4f, 0.125f)); //bas
        Gizmos.DrawWireCube(transform.position + new Vector3(0.08f, 0.1f, 0f), new Vector2(0.125f, 0.4f)); //droite
        Gizmos.DrawWireCube(transform.position + new Vector3(-0.08f, 0.1f, 0f), new Vector2(0.125f, 0.4f)); //gauche*/
    }


    //Move Hero towards target
    private void MoveEnemy()
    {
        //caculate distance between hero and target with consistent speed
        Vector2 direction = (target.transform.position - transform.position).normalized;
        lastMoveDirection = direction;
        // Move hero toward target
        rb.MovePosition(rb.position + direction * baseStats.chargeSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) < 0.13f)
        {
            //Push back hero if overlapping with monster
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, -baseStats.chargeSpeed * Time.deltaTime);
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
        rb.MovePosition(rb.position + direction * baseStats.chargeSpeed * Time.fixedDeltaTime);

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
