using Unity.Burst.Intrinsics;
using UnityEngine;

public class CrossbowAI : MonoBehaviour
{
    [SerializeField]
    CrossbowStats_SO baseStat; // Base stats of slimes
    [SerializeField]
    GameObject arrowPrefab; //Prefab for shot arrow
    CameraManagement cameraStat; // check the stat of the camera
    GameObject target = null; // current hero target
    float timeCooldown; //Time that passes before next attack;

    private void Start()
    {
        cameraStat = Camera.main.GetComponent<CameraManagement>();
    }

    //Check for target, attack if possible
    private void FixedUpdate()
    {
        //If camera is moving do nothing
        if (cameraStat.GetTransitionning()) return;
            //If no target check for one
            if (target == null)
            {
                FindTarget();
            }

            if (target != null)
            {
                //Check if can attack
                if (timeCooldown <= 0)
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
            }

            //attack cooldown
            if (timeCooldown > 0)
            {
                timeCooldown -= Time.deltaTime;
            }

        //Rotate towards target
        if (target != null)
        {
			Vector3 targetPos = target.transform.position;
			targetPos.x -= transform.position.x;
			targetPos.y -= transform.position.y;
			float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg - 90f;
		    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
	}

    //Check if target still alive
    bool CheckTargetAlive()
    {
        return target.activeSelf;
    }

    //Shoot an arrow towards target
    private void DoAttack()
    {
		//Animation
		GetComponent<Animator>().SetTrigger("Shoot");

		//Instantiate an arrow arrow 
		GameObject arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
        //Direction the arrow musr go
        Vector2 direction = (target.transform.position - transform.position).normalized;
        //Turn point of arrow towards target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        //Shoots arrow
        arrow.GetComponent<Rigidbody2D>().linearVelocity = direction * baseStat.arrowSpeed;
        //Start cooldown
        timeCooldown = baseStat.attackCooldown;
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
}
