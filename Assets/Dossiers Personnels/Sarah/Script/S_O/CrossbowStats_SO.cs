using UnityEngine;

[CreateAssetMenu(fileName = "CrossbowStats_SO", menuName = "Scriptable Objects/CrossbowStats_SO")]
public class CrossbowStats_SO : ScriptableObject
{
    [field: SerializeField]
    public int power { get; private set; }
    [field: SerializeField]
    public float attackCooldown { get; private set; }
    [field: SerializeField]
    public float arrowSpeed { get; private set; }
    [field: SerializeField]
    public float kockbackForce { get; private set; }
    [field: SerializeField]
    public int cost { get; private set; }
}
