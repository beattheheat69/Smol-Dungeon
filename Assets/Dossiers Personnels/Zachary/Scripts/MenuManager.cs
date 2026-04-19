using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("DEBUG - Continue")]
    public Button continueButton;
    [SerializeField] bool isContinueAvailable;

    [Header("Evil XP")]
    public TMP_Text evilXPText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        evilXPText.text = "EvilXP = " + EvilXPCount.GetXP().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //FOR DEBUG PURPOSES (check if Continue button is greyed out)
        if (!isContinueAvailable)
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
    }

    public void LoadNewGame()
    {
        //Load the scene with the Dungeon Shaping Screen
        SceneManager.LoadSceneAsync("PlacementEntite");
    }

    public void LoadResumedGame()
    {
        //We'll get there when we get there
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PopUpWindow(Canvas window)
    {
        //Make the window appear
        window.gameObject.SetActive(true);
    }

	public void ToggleFullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	public void QuitGame()
    {
        //Bye bye!
        Application.Quit();
    }
}
