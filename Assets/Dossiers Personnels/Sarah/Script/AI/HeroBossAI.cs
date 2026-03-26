using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroBossAI : Hero
{
    [SerializeField]
    LayerMask colliderLayer; //Layer for object not to overlaps with
    GameObject target = null; // Target enemy will charge
    //Rigidbody2D rb;  //Object rigidbody
    float timeCooldown; //Time that passes before next attack
    bool attacking = false; // hero in attack mode
    public Vector2 lastMoveDirection; // Direction the hero is looking for the attack
    HeroAnimation heroAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        heroAnim = GetComponent<HeroAnimation>();
        rb = GetComponent<Rigidbody2D>();
        FindTarget();
        //health = HeroDataManager.Instance.party[index].currentHealt;  // testing hero health
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && !atTarget)
        {
            MoveHero();
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
            Debug.Log(Vector2.Distance(transform.position, target.transform.position));
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
        lastMoveDirection = (target.transform.position - transform.position).normalized;

        // Move hero toward target
        rb.MovePosition(rb.position + lastMoveDirection * baseStats.chargeSpeed * Time.fixedDeltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) <= 1.233f)
        {
           /* Vector2 pushDirect = ((Vector2)transform.position - (Vector2)target.transform.position).normalized;
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

    //Find which monster is the closest
    private void FindTarget()
    {
        target = GameObject.Find("Boss");
    }
}
