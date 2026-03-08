using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossActions : MonoBehaviour
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

	//Attacks stuff
	RaycastHit2D[] hits;
	public LayerMask masksToHit;
	public float attackRadius = 1f;
	Vector2 attackTransform;
	public float fistSpeed = 1f;
	public float cooldown = 0.25f;
	float timeForNextAttack = 0f;

	private void Start()
	{
		//Gets all player inputs
		playerInput = GetComponent<PlayerInput>();
		basicAttack = playerInput.actions["BasicAction"];
		specialAttackLeft = playerInput.actions["SpecialLeft"];
		specialAttackRight = playerInput.actions["SpecialRight"];
		moveInput = playerInput.actions["Move"];
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
			SpecialAttackRight(lastDir);
			timeForNextAttack = 0;
		}
	}

	void BasicAttack(Vector2 dir) //Basic punch in front of boss
	{
		//Adds current pos to last direction faced to get attack pos
		attackTransform = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
		hits = Physics2D.CircleCastAll(attackTransform, attackRadius, Vector2.zero, masksToHit); //Vector2.zero necessary to not impact attackRadius or circle

		//Checks all colliders hit for hero
		foreach (RaycastHit2D hit in hits)
		{
			Debug.Log(hit.transform.name + " hit!");
			HeroPlaceholderTest heroPlaceholderTest = hit.transform.GetComponent<HeroPlaceholderTest>();
			if (heroPlaceholderTest != null)
				heroPlaceholderTest.TakeDamage(1);
		}
	}

	void SpecialAttackLeft(Vector2 dir) //Shockwave AoE for 1 second
	{
		//Spawns VFX in front of last direction
		GameObject inst = Instantiate(shockwaveAnim);
		inst.transform.position = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
		Destroy(inst, 2f);
	}

	void SpecialAttackRight(Vector2 dir) //Throw fist across room
	{
		GameObject inst = Instantiate(fistAttack);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		inst.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		inst.transform.position = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
		inst.GetComponent<Rigidbody2D>().linearVelocity = dir.normalized * fistSpeed;
		Destroy(inst, 2f);
	}

	private void OnDrawGizmos() //Draws a circle gizmo to display basic attack shape and radius (only works when Gizmos is enabled in play mode)
	{
		Gizmos.DrawWireSphere(attackTransform, attackRadius);
	}
}
