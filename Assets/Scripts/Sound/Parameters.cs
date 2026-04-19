using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parameters : MonoBehaviour
{
	int sceneIndex;
	public StudioEventEmitter musicEmitter;
	static bool musicIsPlaying = false;

	private void Start()
	{
		if (musicEmitter != null && !musicIsPlaying)
		{
			musicEmitter.Play();
			musicIsPlaying = true;	//Cause music to stop playing on multiple playthroughs in editor, should not happen in build. Seems to reset if domain reloads
		}

		sceneIndex = SceneManager.GetActiveScene().buildIndex;
		RuntimeManager.StudioSystem.setParameterByName("ChangeScene", sceneIndex);

		if (sceneIndex == 2)
			RuntimeManager.StudioSystem.setParameterByName("LowPass", 0);
		else
			RuntimeManager.StudioSystem.setParameterByName("LowPass", 1);
	}

	public void SetNormal()
    {
        RuntimeManager.StudioSystem.setParameterByName("LowPass", 0);
    }

    public void SetMax()
    {
		RuntimeManager.StudioSystem.setParameterByName("LowPass", 1);
	}

	private void OnDisable()
	{
		RuntimeManager.StudioSystem.setParameterByName("LowPass", 1);
	}
	//test
}
