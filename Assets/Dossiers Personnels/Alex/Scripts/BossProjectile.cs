using UnityEngine;

public class BossProjectile : MonoBehaviour
{
	public int damage = 1;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Hero") && other.GetComponent<HeroBossAI>() != null)
		{
			Debug.Log(other.transform.name + " hit by flying fist!");
			other.GetComponent<HeroBossAI>().takeDamage(damage, transform.position, 0f); // change 0f value for knockback on hero takeDamage
		}
	}
}
