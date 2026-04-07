using System.Collections;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
	PlayerMovement playerMovement;
	Character characterAI;
	Animator anim;
	Vector2 dir;
	Vector2 dirAI;
	Vector2 lastDir;

	private void Start()
	{
		playerMovement = GetComponent<PlayerMovement>();
		characterAI = GetComponent<Character>();
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		if (playerMovement.enabled)
		{
			dir = playerMovement.GetDirection();

			if (dir != Vector2.zero)
			{
				lastDir = dir;
				anim.SetBool("isWalking", true);
				AnimateWalk(dir);
			}
			else
			{
				anim.SetBool("isWalking", false);
				AnimateWalk(lastDir);
			}
		}
		else
		{
			dirAI = characterAI.lastMoveDirection;

			if (dirAI != Vector2.zero)
			{
				lastDir = dirAI;
				anim.SetBool("isWalking", true);
				AnimateWalk(dirAI);
			}
			else
			{
				lastDir = dirAI;
				anim.SetBool("isWalking", false);
				AnimateWalk(lastDir);
			}
		}
	}

	void AnimateWalk(Vector2 direction)
	{
		anim.SetFloat("dirX", direction.x);
		anim.SetFloat("dirY", direction.y);
	}

	public IEnumerator MonsterAttack()
	{
		//soundCaster.PlayAttackSFX();
		anim.SetTrigger("Attack");
		//playerMovement.enabled = false;
		yield return new WaitForSeconds(0.5f);
		//playerMovement.enabled = true;
	}
}
