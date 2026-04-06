using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStats_SO", menuName = "Scriptable Objects/MonsterStats_SO")]
public abstract class MonsterStats_SO : ScriptableObject
{
    //for a stat that will be all the same for all monsters
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
    [field: SerializeField]
    public float kockbackForce { get; private set; }
    [field: SerializeField]
    public int cost { get; private set; }

}
