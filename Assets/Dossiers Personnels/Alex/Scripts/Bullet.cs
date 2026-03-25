using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
	public int damage = 1;

	private void OnTriggerEnter2D(Collider2D other)
	{
		//if (other.CompareTag("Hero") && other.GetComponent<HeroPlaceholderTest>() != null && other.GetComponent<HeroPlaceholderTest>().enabled == true)
		//	other.GetComponent<HeroPlaceholderTest>().TakeDamage(damage);

		//if (other.gameObject.TryGetComponent(out IDamageable hitTarget)) //BUG: One shots hero
		//{
		//	hitTarget.takeDamage(damage);
		//}

		if (other.CompareTag("Hero"))
		{
			HeroAI heroAI = other.transform.GetComponent<HeroAI>();
			heroAI.takeDamage(damage, transform.position, 0f); // change 0f value for knockback on hero takeDamage
        }
		else if (other.CompareTag("Monster"))
		{
			MonsterAI monsterAI = other.transform.GetComponent<MonsterAI>();
			monsterAI.takeDamage(damage, transform.position, 0f); // change 0f value for knockback on hero takeDamage
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
