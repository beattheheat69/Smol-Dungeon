using UnityEngine;

public class SlimeBounceAttack : MonoBehaviour
{
	public MonsterStats_SO slimeSO;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Hero"))
			other.gameObject.GetComponent<Hero>().takeDamage(slimeSO.power, transform.position, slimeSO.kockbackForce);
	}
}
