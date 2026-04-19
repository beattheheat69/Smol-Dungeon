using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class RunStatus : MonoBehaviour
{
	public GameObject loseText;
	public GameObject winText;
	public GameObject xpText;
	bool bossWin;
	GameObject thingsToSend;
	bool restarting;

	private void Start()
	{
		thingsToSend = GameObject.Find("ThingsToSend").gameObject;
		restarting = false;
	}

	public void CallRestart(bool win, int day)
	{
		if (!restarting)
		{
			bossWin = win;
			//StartCoroutine(RestartTheGame(win, day));
			if (win)
			{
				winText.SetActive(true);
				EvilXPCount.GainXP(100); //XP gain based on 100 + EP left when starting run
			}
			else if (!win)
			{
				loseText.SetActive(true);
				EvilXPCount.GainXP(50); //XP gain based on 100 + EP left when starting run
			}

			//Add Evil XP here
			xpText.SetActive(true);
			//EvilXPCount.GainXP(100); //XP gain based on 100 + EP left when starting run
			StartCoroutine(GetComponent<CountingUp>().CountUp());
		}

		restarting = true;
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
		if (thingsToSend != null)
			Destroy(thingsToSend);

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

	}
}
