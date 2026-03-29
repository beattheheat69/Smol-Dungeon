using UnityEngine;

public class BossProjectile : MonoBehaviour
{
	public MonsterStats_SO bossStats;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Hero") && other.GetComponent<HeroBossAI>() != null)
		{
			Debug.Log(other.transform.name + " hit by boss!");
			other.GetComponent<HeroBossAI>().takeDamage(bossStats.power, transform.position, 0f); // change 0f value for knockback on hero takeDamage
		}
	}
}
