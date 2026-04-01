using UnityEngine;

public class ArmorAI : MonsterAI
{
    [SerializeField]
    Armor_Stats_SO baseStats; // Base stats of slime
    [SerializeField]
    LayerMask colliderLayer;
    CameraManagement cameraStat; // check the stat of the camera
    GameObject target = null; // current hero target
    //Rigidbody2D rb;  //Object rigidbody
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // monster in attack mode
    bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = baseStats.health;
        power = baseStats.power;
        cameraStat = Camera.main.GetComponent<CameraManagement>();
    }

    // Update is called once per frame
    void Update()
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
        if (isActive && collision.gameObject == target)
        {
            if (collision.gameObject == target && !attacking)
            {
                attacking = true;
                atTarget = true;
                rb.linearVelocity = Vector2.zero;
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
        Vector2 direction = (target.transform.position - transform.position).normalized;
        // Move hero toward target
        rb.MovePosition(rb.position + direction * baseStats.chargeSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) < 1f)
        {
            /*Vector2 pushDirect = ((Vector2)transform.position - (Vector2)target.transform.position).normalized;
            transform.position += (Vector3)pushDirect * baseStats.chargeSpeed * Time.deltaTime;*/
            atTarget = true;
        }
        //Correctoverlap();
    }

   /* private void Correctoverlap()
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
    }*/


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
