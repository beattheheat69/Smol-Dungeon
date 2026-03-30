using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
	//Works in theory
	//***Need to test once sfx and music are in main menu***

	[SerializeField] Slider sfxSlider;
	[SerializeField] Slider musicSlider;
	Bus sfxBus;
	Bus musicBus;

	private void Start()
	{
		sfxBus = RuntimeManager.GetBus("bus:/SFX");
		musicBus = RuntimeManager.GetBus("bus:/Music");
	}

	public void MusicVolume()
	{
		musicBus.setVolume(musicSlider.value);
	}

	public void SFXVolume()
	{
		sfxBus.setVolume(sfxSlider.value);
	}
}
