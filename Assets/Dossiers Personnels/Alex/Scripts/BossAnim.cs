using System.Collections;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
	PlayerMovement playerMovement;
	Animator anim;
	Vector2 dir;
	Vector2 lastDir;

	private void Start()
	{
		playerMovement = GetComponent<PlayerMovement>();
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		dir = playerMovement.GetDirection();

		if (dir != Vector2.zero)
		{
			lastDir = dir;
			anim.SetBool("isWalking", true);
			AnimateBoss(dir);
		}
		else
		{
			anim.SetBool("isWalking", false);
			AnimateBoss(lastDir);
		}

	}

	void AnimateBoss(Vector2 direction)
	{
		anim.SetFloat("dirX", direction.x);
		anim.SetFloat("dirY", direction.y);
	}

	public IEnumerator BossAttacks()
	{
		anim.SetTrigger("BasicAttack");
		playerMovement.enabled = false;
		yield return new WaitForSeconds(0.5f);
		playerMovement.enabled = true;
	}

	public IEnumerator BossProjectile()
	{
		anim.SetTrigger("Projectile");
		playerMovement.enabled = false;
		yield return new WaitForSeconds(0.5f);
		playerMovement.enabled = true;
	}

	public IEnumerator BossShockwave()
	{
		anim.SetTrigger("Shockwave");
		playerMovement.enabled = false;
		yield return new WaitForSeconds(0.5f);
		playerMovement.enabled = true;
	}
}
