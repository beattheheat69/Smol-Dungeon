using UnityEngine;

public class HeroAnimation : MonoBehaviour
{
    Hero heroAI;
	Animator heroAnim;
	//public Animator swordAnim;
	Vector2 direction;

	private void Start()
	{
		heroAI = GetComponent<Hero>();
		heroAnim = GetComponent<Animator>();
		//swordAnim = GameObject.Find("Sword").GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		direction = heroAI.lastMoveDirection; //Bug: Grabs 0 in entrance, find correct var to get movement

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

		//swordAnim.SetFloat("dirX", direction.x);
		//swordAnim.SetFloat("dirY", direction.y);
	}

	public void IsAttacking()
	{
		heroAnim.SetTrigger("Attack");
		//swordAnim.SetTrigger("Slash");
	}
}
