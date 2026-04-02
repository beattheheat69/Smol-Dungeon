using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class RunStatus : MonoBehaviour
{
	public TMP_Text loseText;
	public TMP_Text winText;

	public void CallRestart(bool win, int day)
	{
		StartCoroutine(RestartTheGame(win, day));
		if (win)
			winText.enabled = true;
		else if (!win)
			loseText.enabled = true;
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
}
