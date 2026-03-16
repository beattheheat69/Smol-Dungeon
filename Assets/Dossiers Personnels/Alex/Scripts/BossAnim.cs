using UnityEngine;

public class BossAnim : MonoBehaviour
{
	PlayerMovement playerMovement;
	Animator anim;
	Vector2 dir;

	private void Start()
	{
		playerMovement = GetComponent<PlayerMovement>();
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		dir = playerMovement.GetDirection();
		AnimateBoss(dir);
	}

	void AnimateBoss(Vector2 direction)
	{
		anim.SetBool("isWalking", true);
		anim.SetFloat("DirX", direction.x);
		anim.SetFloat("DirY", direction.y);
	}

	public void BossAttacks()
	{
		anim.SetTrigger("Attack");
	}
}
