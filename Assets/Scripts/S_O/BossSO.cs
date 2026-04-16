using UnityEngine;

[CreateAssetMenu(fileName = "BossStats_SO", menuName = "Scriptable Objects/BossStats_SO")]
public class BossSO : MonsterStats_SO
{
    [SerializeField] public float SpecialAttack1Cooldown;
    [SerializeField] public float SpecialAttack2Cooldown;
	[SerializeField] public int SpecialAttack1Damage;
	[SerializeField] public int SpecialAttack2Damage;
	[SerializeField] public float SpecialAttack1Knockback;
	[SerializeField] public float SpecialAttack2Knockback;
}
