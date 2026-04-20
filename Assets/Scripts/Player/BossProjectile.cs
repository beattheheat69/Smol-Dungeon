using UnityEngine;

public class BossProjectile : MonoBehaviour
{
	public BossSO bossStats;
	int damage;
	float knockback;
	public Vector2 attackPos;

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
			attackPos = transform.position;
        }

		else if (CompareTag("BossAttack2"))
		{
			damage = bossStats.SpecialAttack2Damage;
            knockback = bossStats.SpecialAttack2Knockback; // add force if needed with +
			attackPos = transform.position;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (CompareTag("BossAttack"))
			attackPos = GetComponentInParent<Transform>().position;

		if (other.CompareTag("Hero") && other.GetComponent<HeroBossAI>() != null)
		{
			StartCoroutine(HandyFunctions.HitStop());
			other.GetComponent<HeroBossAI>().takeDamage(damage, attackPos, knockback); // change 0f value for knockback on hero takeDamage
		}
	}
}