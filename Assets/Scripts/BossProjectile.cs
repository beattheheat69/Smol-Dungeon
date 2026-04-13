using UnityEngine;

public class BossProjectile : MonoBehaviour
{
	public BossSO bossStats;
	int damage;

	private void Start()
	{
		if (CompareTag("BossAttack"))
			damage = bossStats.power;
		else if (CompareTag("BossAttack1"))
			damage = bossStats.SpecialAttack1Damage;
		else if (CompareTag("BossAttack2"))
			damage = bossStats.SpecialAttack2Damage;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Hero") && other.GetComponent<HeroBossAI>() != null)
		{
			other.GetComponent<HeroBossAI>().takeDamage(damage, transform.position, 0f); // change 0f value for knockback on hero takeDamage
		}
	}
}
