using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RunStatus : MonoBehaviour
{
	public IEnumerator RestartTheGame()
	{
		Debug.Log("You Win!");
		yield return new WaitForSeconds(2);
		SceneManager.LoadSceneAsync("MainMenu");
	}
}
