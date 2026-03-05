using UnityEngine;

public class HeroPlaceholderTest : MonoBehaviour
{
    public int heroHealth = 3;

	public void TakeDamage(int dmgAmount)
	{
		heroHealth -= dmgAmount;
		if (heroHealth <= 0)
			Destroy(gameObject);
	}
}
