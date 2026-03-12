using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int damage = 1;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Hero") && other.GetComponent<HeroPlaceholderTest>() != null && other.GetComponent<HeroPlaceholderTest>().enabled == true)
			other.GetComponent<HeroPlaceholderTest>().TakeDamage(damage);

		if (other.gameObject.TryGetComponent(out IDamageable hitTarget))
		{
			hitTarget.takeDamage(damage);
		}

		if (other.tag != "Room")
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Room")
		{
			Destroy(gameObject);
		}
	}
}
