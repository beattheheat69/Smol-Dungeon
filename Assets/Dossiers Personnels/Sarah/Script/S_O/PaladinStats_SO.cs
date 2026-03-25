using UnityEngine;

[CreateAssetMenu(fileName = "PaladinStats_SO", menuName = "Scriptable Objects/PaladinStats_SO")]
public class PaladinStats_SO : ScriptableObject
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
    [field: SerializeField]
    public float dodgeChange { get; private set; }
    [field: SerializeField]
    public int Nbpotion { get; private set; }
    [field: SerializeField]
    public float kockbackForce { get; private set; }
    //permanent buffs (and debuffs)
}
