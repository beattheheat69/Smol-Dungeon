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

	//Attacks stuff
	RaycastHit2D[] hits;
	public LayerMask masksToHit;
	public float attackRadius = 1f;
	Vector2 attackTransform;

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

		//Basic attack input & function call
		if (basicAttack.WasPressedThisFrame())
		{
			Debug.Log("Basic Attack");
			BasicAttack();
		}

		//Special attack 1 input & function call
		if (specialAttackLeft.WasPressedThisFrame())
		{
			Debug.Log("Special Attack Left");
			SpecialAttackLeft(lastDir);
		}

		//Special attack 2 input & function call
		if (specialAttackRight.WasPressedThisFrame())
		{
			Debug.Log("Special Attack Right");
			SpecialAttackRight(lastDir);
		}
	}

	void BasicAttack()
	{

	}

	void SpecialAttackLeft(Vector2 dir)
	{
		//Spawns VFX in front of last direction
		GameObject inst = Instantiate(shockwaveAnim);
		inst.transform.position = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
		attackTransform = inst.transform.position;
		Destroy(inst, 2f);

		//Ray cast maybe in coroutine to do trigger collider
		hits = Physics2D.CircleCastAll(attackTransform, attackRadius, Vector2.zero, masksToHit); //Vector2.zero necessary to not impact attackRadius or circle

		foreach (RaycastHit2D hit in hits)
		{
			Debug.Log(hit.transform.name + " hit!");
			HeroPlaceholderTest heroPlaceholderTest = hit.transform.GetComponent<HeroPlaceholderTest>();
			if (heroPlaceholderTest != null)
				heroPlaceholderTest.TakeDamage(1);
		}
	}

	void SpecialAttackRight(Vector2 dir)
	{

	}

	private void OnDrawGizmos() //Draws a circle gizmo to display attack shape and radius (only works when Gizmos is enabled in play mode)
	{
		Gizmos.DrawWireSphere(attackTransform, attackRadius);
	}
}
