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

    [Header("Pause Menu")]
    [SerializeField] Canvas pauseWindowPrefab;
    [HideInInspector] public bool isGamePaused = false;
    public TMP_Text evilXPTextPlay;
    public TMP_Text evilXPTextUpgrade;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        evilXPTextPlay.text = "EvilXP = " + EvilXPCount.GetXP().ToString();
        evilXPTextUpgrade.text = "EvilXP = " + EvilXPCount.GetXP().ToString();
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

    public void UpdateEvilXP()
    {
        evilXPTextPlay.text = "EvilXP = " + EvilXPCount.GetXP().ToString();
        evilXPTextUpgrade.text = "EvilXP = " + EvilXPCount.GetXP().ToString();
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

    public void SetFullScreen (bool fullscreen)
    {
        if (fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
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
