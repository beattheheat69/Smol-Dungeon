using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class RunStatus : MonoBehaviour
{
	public GameObject loseText;
	public GameObject winText;

	public void CallRestart(bool win, int day)
	{
		//StartCoroutine(RestartTheGame(win, day));
		if (win)
			winText.SetActive(true);
		else if (!win)
			loseText.SetActive(true);
	}

	public IEnumerator RestartTheGame(bool win, int day)
	{
		yield return new WaitForSeconds(2);
		if (win && day == 1 )
		{
			SceneManager.LoadSceneAsync("PlacementEntite");
		}
		else 
		{
			Destroy(HeroDataManager.Instance);
            SceneManager.LoadSceneAsync("MainMenu");
        }
		
	}

	public void CallRestartManual(bool win)
	{
		if (win)
		{
			SceneManager.LoadSceneAsync("PlacementEntite");
		}
		else
		{
			Destroy(HeroDataManager.Instance);
			SceneManager.LoadSceneAsync("MainMenu");
		}

		//Add Evil XP here
		EvilXPCount.GainXP(34);
	}
}
