using UnityEngine;

[CreateAssetMenu(fileName = "SpikeStats_SO", menuName = "Scriptable Objects/SpikeStats_SO")]
public class SpikeStats_SO : ScriptableObject
{
    [SerializeField]
    public float timeUp; //How long the spike stay up
    [SerializeField]
    public float timeDown;  // How long the spike stay down
    [SerializeField]
    public float startDelay; // Offset for all spikes
    [SerializeField]
    public int power; //damage power
    [SerializeField]
    public float attackCooldown; //damage cooldown
}
