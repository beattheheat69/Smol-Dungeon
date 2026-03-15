using UnityEngine;

public class ShockWaveAttack : MonoBehaviour
{
	public int damage = 1;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Hero") && other.GetComponent<HeroBossAI>() != null)
		{
			other.GetComponent<HeroBossAI>().takeDamage(damage);
			Debug.Log(other.transform.name + " hit by shockwave!");
		}
	}
}
