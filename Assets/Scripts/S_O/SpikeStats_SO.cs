using UnityEngine;

[CreateAssetMenu(fileName = "SpikeStats_SO", menuName = "Scriptable Objects/SpikeStats_SO")]
public class SpikeStats_SO : ScriptableObject
{
    [field: SerializeField]
    public float timeUp { get; private set; } //How long the spike stay up
    [field: SerializeField]
    public float timeDown { get; private set; }  // How long the spike stay down
    [field: SerializeField]
    public float startDelay { get; private set; } // Offset for all spikes
    [field: SerializeField]
    public int power { get; private set; } //damage power
    [field: SerializeField]
    public float attackCooldown; //damage cooldown
    [field: SerializeField]
    public float kockbackForce { get; private set; }
    [field: SerializeField]
    public int cost { get; private set; }
}
