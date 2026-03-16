using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class RunStatus : MonoBehaviour
{
	public TMP_Text loseText;
	public TMP_Text winText;

	public void CallRestart(bool win)
	{
		StartCoroutine(RestartTheGame());
		if (win)
			winText.enabled = true;
		else if (!win)
			loseText.enabled = true;
	}

	public IEnumerator RestartTheGame()
	{
		yield return new WaitForSeconds(2);
		SceneManager.LoadSceneAsync("MainMenu");
	}
}
