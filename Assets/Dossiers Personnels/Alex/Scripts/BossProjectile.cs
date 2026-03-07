using UnityEngine;

public class BossProjectile : MonoBehaviour
{
	public int damage = 1;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Hero") && other.GetComponent<HeroPlaceholderTest>() != null)
			other.GetComponent<HeroPlaceholderTest>().TakeDamage(damage);

		Debug.Log(other.transform.name + " hit!");
	}
}
