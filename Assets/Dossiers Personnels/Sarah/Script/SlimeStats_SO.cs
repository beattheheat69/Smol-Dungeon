using UnityEngine;

[CreateAssetMenu(fileName = "SlimeStats_SO", menuName = "Scriptable Objects/SlimeStats_SO")]
public class SlimeStats_SO : ScriptableObject
{
    [field: SerializeField]
    public float chargeSpeed { get; private set; }
    [field: SerializeField]
    public int health { get; private set; }
    [field: SerializeField]
    public int power { get; private set; }
    [field: SerializeField]
    public float attackCooldown { get; private set; }
    [field: SerializeField]
    public float attackChance { get; private set; }
}
