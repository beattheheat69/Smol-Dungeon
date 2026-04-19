using UnityEngine;

public class SlimeBounceAttack : MonoBehaviour
{
	public MonsterStats_SO slimeSO;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Hero"))
			other.GetComponent<Hero>().takeDamage(slimeSO.power, transform.position, slimeSO.kockbackForce);
	}
}
