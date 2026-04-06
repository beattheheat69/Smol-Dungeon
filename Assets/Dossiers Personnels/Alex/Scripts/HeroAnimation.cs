using FMODUnity;
using UnityEngine;

public class HeroAnimation : MonoBehaviour
{
    Character characterAI;
	Animator heroAnim;
	Vector2 direction;

	private void Start()
	{
        characterAI = GetComponent<Character>();
		heroAnim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		direction = characterAI.lastMoveDirection; //Bug: Grabs 0 in entrance, find correct var to get movement

		if (direction != Vector2.zero)
		{
			heroAnim.SetBool("isWalking", true);
		}
		else
		{
			heroAnim.SetBool("isWalking", false);
		}

		heroAnim.SetFloat("dirX", direction.x);
		heroAnim.SetFloat("dirY", direction.y);
	}

	public void IsAttacking()
	{
		GetComponent<SoundCaster>().PlayAttackSFX();
		heroAnim.SetTrigger("Attack");
	}
}
