using FMODUnity;
using UnityEngine;

public class Parameters : MonoBehaviour
{
	private void Start()
	{
		RuntimeManager.StudioSystem.setParameterByName("LowPass", 0);
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
}
