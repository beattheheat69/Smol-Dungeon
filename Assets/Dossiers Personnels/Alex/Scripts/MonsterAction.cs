using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterAction : MonoBehaviour
{
	//Script specific for monster actions

	//Inputs
	PlayerInput playerInput;
	InputAction basicActionInput;

	//Attacks stuff
	RaycastHit2D[] hits;
	public LayerMask masksToHit;
	public float attackRadius = 0.5f;
	Vector2 attackTransform;
	public float cooldown = 0.5f;
	float timeForNextAttack = 0f;

	private void Start()
	{
		//Input references
		playerInput = GetComponent<PlayerInput>();
		basicActionInput = playerInput.actions["BasicAction"];
	}

	private void Update()
	{
		//Cooldown timer
		if (timeForNextAttack < cooldown) //Maybe individual cooldowns for each attack?
			timeForNextAttack += Time.deltaTime;

		//Input and function call for basic action
		if (basicActionInput.WasPressedThisFrame() && cooldown <= timeForNextAttack)
		{
			BasicAttack(GetComponent<PlayerMovement>().lastDir);
			timeForNextAttack = 0;
		}
	}

	void BasicAttack(Vector2 dir)
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

	private void OnDrawGizmos() //Draws a circle gizmo to display basic attack shape and radius (only works when Gizmos is enabled in play mode)
	{
		Gizmos.DrawWireSphere(attackTransform, attackRadius);
	}
}
