using FMODUnity;
using UnityEngine;

public class SoundCaster : MonoBehaviour
{
    [SerializeField] EventReference attackSFX;
	[SerializeField] EventReference attack2SFX;
	[SerializeField] EventReference attack3SFX;
	[SerializeField] EventReference hitSFX;
    [SerializeField] EventReference blockSFX;

	public void PlayAttackSFX()
    {
        RuntimeManager.PlayOneShot(attackSFX);
    }

    public void PlayAttack2SFX()
    {
        RuntimeManager.PlayOneShot(attack2SFX);
    }

    public void PlayAttack3SFX()
    {
        RuntimeManager.PlayOneShot(attack3SFX);
    }

    public void PlayHitSFX()
    {
        RuntimeManager.PlayOneShot(hitSFX);
    }

    public void PlayBlockSFX()
    {
		RuntimeManager.PlayOneShot(blockSFX);
	}
}
