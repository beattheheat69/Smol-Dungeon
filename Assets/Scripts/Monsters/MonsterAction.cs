using System.Collections;
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
	Rigidbody2D rb;

	public MonsterStats_SO baseStats;

	private void Start()
	{
		//Input references
		playerInput = GetComponent<PlayerInput>();
		basicActionInput = playerInput.actions["BasicAction"];
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		//Cooldown timer
		if (timeForNextAttack < baseStats.attackCooldown) //cooldown for each monster
			timeForNextAttack += Time.deltaTime;

		//Input and function call for basic action
		if (basicActionInput.WasPressedThisFrame() && baseStats.attackCooldown <= timeForNextAttack)
		{
			BasicAttack(GetComponent<PlayerMovement>().lastDir);
			timeForNextAttack = 0;
		}
	}

	void BasicAttack(Vector2 dir)
	{
        GetComponent<Animator>().SetTrigger("Attack");
		GetComponent<SoundCaster>().PlayAttackSFX();

		if (CompareTag("Monster"))
		{
			StartCoroutine(BounceAttack(dir));
		}
		else
		{
			attackTransform = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
			hits = Physics2D.CircleCastAll(attackTransform, attackRadius, Vector2.zero, 0, masksToHit); //Vector2.zero necessary to not impact attackRadius or circle

			//Checks all colliders hit for hero
			foreach (RaycastHit2D hit in hits)
			{
				Debug.Log(hit.transform.name + " hit!");
				HeroAI heroAI = hit.transform.GetComponent<HeroAI>();
				heroAI.takeDamage(baseStats.power, transform.position, 0f); // change 0f value for knockback on hero takeDamage
			}
		}
			//Adds current pos to last direction faced to get attack pos
	}

	IEnumerator BounceAttack(Vector2 direction)
	{
		// Use Impulse to give it immediate speed
		rb.AddForce(direction * 3f, ForceMode2D.Impulse);

		// Wait for the duration of the dash
		yield return new WaitForSeconds(0.4f);

		// Stop the slime for short knockback
		rb.linearVelocity = Vector2.zero;
	}
}
