using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossProjectile : MonoBehaviour
{
	public BossSO bossStats;
	int damage;
	float knockback;

	private void Start()
	{
		if (CompareTag("BossAttack"))
		{
			damage = bossStats.power;
			knockback = bossStats.kockbackForce; // add force if needed with +
        }

		else if (CompareTag("BossAttack1"))
		{
			damage = bossStats.SpecialAttack1Damage;
			knockback = bossStats.SpecialAttack1Knockback; // add force if needed with +
        }

		else if (CompareTag("BossAttack2"))
		{
			damage = bossStats.SpecialAttack2Damage;
            knockback = bossStats.SpecialAttack2Knockback; // add force if needed with +
        }
			
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Hero") && other.GetComponent<HeroBossAI>() != null)
		{
			StartCoroutine(HandyFunctions.HitStop());
			other.GetComponent<HeroBossAI>().takeDamage(damage, transform.position, knockback); // change 0f value for knockback on hero takeDamage
		}
	}
}
