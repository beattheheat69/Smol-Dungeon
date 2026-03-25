using UnityEngine;

public interface IDamageable
{
    void takeDamage(int damage, Vector2 attackerPosition, float knockbackStrength);
}
