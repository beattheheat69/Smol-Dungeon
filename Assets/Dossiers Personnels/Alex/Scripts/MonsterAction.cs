using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

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

	[SerializeField] SlimeStats_SO baseStats;

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
		GetComponent<Animator>().SetTrigger("Attack");

		//Adds current pos to last direction faced to get attack pos
		attackTransform = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
		hits = Physics2D.CircleCastAll(attackTransform, attackRadius, Vector2.zero, 0, masksToHit); //Vector2.zero necessary to not impact attackRadius or circle

		//Checks all colliders hit for hero
		foreach (RaycastHit2D hit in hits)
		{
			Debug.Log(hit.transform.name + " hit!");
			//if (heroAI.TryGetComponent(out IDamageable hitTarget)) //BUG: One shots hero
			//	hitTarget.takeDamage(baseStats.power);
			HeroAI heroAI = hit.transform.GetComponent<HeroAI>();
			heroAI.takeDamage(baseStats.power);
		}
	}

	private void OnDrawGizmos() //Draws a circle gizmo to display basic attack shape and radius (only works when Gizmos is enabled in play mode)
	{
		Gizmos.DrawWireSphere(attackTransform, attackRadius);
	}
}
