using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
	//***IMPORTANT***
	//Slider values must be between 0 and 1 to adjust fmod bus/vca properties

	[SerializeField] Slider sfxSlider;
	[SerializeField] Slider musicSlider;
	VCA sfxBus;
	VCA musicBus;

	private void Start()
	{
		sfxBus = RuntimeManager.GetVCA("vca:/SFX");
		musicBus = RuntimeManager.GetVCA("vca:/Music");
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
