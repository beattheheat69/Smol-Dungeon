using System.Collections;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
	PlayerMovement playerMovement;
	Animator anim;
	Vector2 dir;
	Vector2 lastDir;
	SoundCaster soundCaster;

	private void Start()
	{
		playerMovement = GetComponent<PlayerMovement>();
		anim = GetComponent<Animator>();
		soundCaster = GetComponent<SoundCaster>();
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
		soundCaster.PlayAttackSFX();
		anim.SetTrigger("BasicAttack");
		playerMovement.enabled = false;
		yield return new WaitForSeconds(0.5f);
		playerMovement.enabled = true;
	}

	public IEnumerator BossProjectile()
	{
		soundCaster.PlayAttack2SFX();
		anim.SetTrigger("Projectile");
		playerMovement.enabled = false;
		yield return new WaitForSeconds(0.5f);
		playerMovement.enabled = true;
	}

	public IEnumerator BossShockwave()
	{
		soundCaster.PlayAttack3SFX();
		anim.SetTrigger("Shockwave");
		playerMovement.enabled = false;
		yield return new WaitForSeconds(0.5f);
		playerMovement.enabled = true;
	}
}
