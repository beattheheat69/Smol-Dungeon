using Mono.Cecil.Cil;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossActions : MonoBehaviour, IDamageable
{
	//Controls boss' actions

	//Player inputs
    PlayerInput playerInput;
    InputAction basicAttack;
    InputAction specialAttackLeft;
    InputAction specialAttackRight;
	InputAction moveInput;

	//Facing direction for movement and action direction
	Vector2 moveDir;
	Vector2 lastDir;

	//Reference to action game object for instantiate
	public GameObject shockwaveAnim;
	public GameObject fistAttack;
    GameManager gm;

    //Attacks stuff
    RaycastHit2D[] hits;
	public LayerMask masksToHit;
	public float attackRadius = 1f;
	Vector2 attackTransform;
	public float fistSpeed = 1f;
	public float cooldown = 0.25f;
	float timeForNextAttack = 0f;

	//Lifebar thingy
	public Lifebar lifebar;

	[SerializeField] MonsterStats_SO stats;
	[SerializeField] int health;

	private void Start()
	{
		//Gets all player inputs
		playerInput = GetComponent<PlayerInput>();
		basicAttack = playerInput.actions["BasicAction"];
		specialAttackLeft = playerInput.actions["SpecialLeft"];
		specialAttackRight = playerInput.actions["SpecialRight"];
		moveInput = playerInput.actions["Move"];
		health = stats.health;
        gm = FindFirstObjectByType<GameManager>();

        //Initiate the health bar
        lifebar.SetMaxHealth(health);
	}

	private void Update()
	{
		//Reads movement input as Vector2
		moveDir = moveInput.ReadValue<Vector2>();

		//Stores last direction moved to keep facing direction
		if (moveDir != Vector2.zero)
			lastDir = moveDir;

		//Cooldown timer
		if (timeForNextAttack < cooldown) //Maybe individual cooldowns for each attack?
			timeForNextAttack += Time.deltaTime;

		//Basic attack input & function call
		if (basicAttack.WasPressedThisFrame() && cooldown <= timeForNextAttack)
		{
			Debug.Log("Basic Attack");
			BasicAttack(lastDir);
			timeForNextAttack = 0;
		}

		//Special attack 1 input & function call
		if (specialAttackLeft.WasPressedThisFrame() && cooldown <= timeForNextAttack)
		{
			Debug.Log("Special Attack Left");
			SpecialAttackLeft(lastDir);
			timeForNextAttack = 0;
		}

		//Special attack 2 input & function call
		if (specialAttackRight.WasPressedThisFrame() && cooldown <= timeForNextAttack)
		{
			Debug.Log("Special Attack Right");
			StartCoroutine(SpecialAttackRight(lastDir));
			timeForNextAttack = 0;
		}

		//Only works when called here
		lifebar.SetHealth(health);
	}

	void BasicAttack(Vector2 dir) //Basic punch in front of boss
	{
		StartCoroutine(GetComponent<BossAnim>().BossAttacks());

		////Replaced by a trigger collider in BasicAttackCollider child object of Boss
		////Adds current pos to last direction faced to get attack pos
		//attackTransform = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
		//hits = Physics2D.CircleCastAll(attackTransform, attackRadius, Vector2.zero, 0, masksToHit); //Vector2.zero necessary to not impact attackRadius or circle

		////Checks all colliders hit for hero
		//foreach (RaycastHit2D hit in hits)
		//{
		//	Debug.Log(hit.transform.name + " hit!");
		//	HeroBossAI heroBossAI = hit.transform.GetComponent<HeroBossAI>();
		//	heroBossAI.takeDamage(stats.power, transform.position, 0f);  // The last one is the knockback force, change value if you want knockback
		//	//if (heroBossAI.TryGetComponent(out IDamageable hitTarget)) //BUG: one shots the hero
		//	//	hitTarget.takeDamage(stats.power);
		//}
	}

	void SpecialAttackLeft(Vector2 dir) //Shockwave AoE for 1 second
	{
		StartCoroutine(GetComponent<BossAnim>().BossShockwave());

		if (dir == Vector2.zero) //Default to south direction in case no move was made when scene starts
			dir = Vector2.down;

		//Spawns VFX in front of last direction
		GameObject inst = Instantiate(shockwaveAnim);
		if (dir != Vector2.up)
			inst.transform.position = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y - 1);
		else
			inst.transform.position = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);

		Destroy(inst, 1f);
	}

	IEnumerator SpecialAttackRight(Vector2 dir) //Throw fist across room
	{
		StartCoroutine(GetComponent<BossAnim>().BossProjectile());
		yield return new WaitForSeconds(0.25f);
		GameObject inst = Instantiate(fistAttack);

		if (dir == Vector2.zero) //Default to south direction in case no move was made when scene starts
			dir = Vector2.down;

		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
		inst.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		inst.transform.position = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
		inst.GetComponent<Rigidbody2D>().linearVelocity = dir.normalized * fistSpeed;
		Destroy(inst, 2f);
	}

	private void OnDrawGizmos() //Draws a circle gizmo to display basic attack shape and radius (only works when Gizmos is enabled in play mode)
	{
		Gizmos.DrawWireSphere(attackTransform, attackRadius);
	}


	
	public void takeDamage(int damage, Vector2 attackerPosition, float knockbackStrength) // attackPosition and knockbackStrenght is for getting knockback on hit, don't the the boss will so didn't implement the code for it. If neede exemple in Character and Hero script
	{
        health -= damage;
		lifebar.SetHealth(health);
		if (health <= 0)
		{
			//Defeat
			GameObject.Find("GameManager").GetComponent<RunStatus>().CallRestart(false, gm.GetDay());
			gameObject.SetActive(false);
		}
	}
}
