using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class RunStatus : MonoBehaviour
{
	public GameObject loseText;
	public GameObject winText;
	bool bossWin;

	public void CallRestart(bool win, int day)
	{
		bossWin = win;
		//StartCoroutine(RestartTheGame(win, day));
		if (win)
			winText.SetActive(true);
		else if (!win)
			loseText.SetActive(true);
	}

	public IEnumerator RestartTheGame(bool win, int day)
	{
		yield return new WaitForSeconds(0.5f);
		if (win && day == 2)
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
		Destroy(GameObject.Find("ThingsToSend").gameObject);
		//if (win)
		//{
		//	SceneManager.LoadSceneAsync("PlacementEntite"); //Change to MainMenu si pas capable de fix 2nd day missing entities bug
		//}
		//else
		//{
		//	Destroy(HeroDataManager.Instance);
		//	SceneManager.LoadSceneAsync("MainMenu");
		//}

		StartCoroutine(RestartTheGame(bossWin, HeroDataManager.Instance.GetDay()));

		//Add Evil XP here
		EvilXPCount.GainXP(34); //XP gain based on 100 + EP left when starting run
	}
}
