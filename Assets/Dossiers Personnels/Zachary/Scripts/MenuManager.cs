using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Pause est gérer par son propre script, pas besoin de garder celui-ci, il donne des errors car il trouve pas le continue button
        //DontDestroyOnLoad(this);
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

        //FOR DEBUG PURPOSES (Test Pause Menu)

        //If the Pause button is pressed
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            //Pause the game
            PauseGame();
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

    public void PauseGame()
    {

        if (isGamePaused) { 
            Time.timeScale = 1.0f; //Unfreeze the action
            pauseWindowPrefab.gameObject.SetActive(false); //Remove the pause menu window
            isGamePaused = false; //Set the boolean value to signal the game is unpaused
        }

        else
        {
            Time.timeScale = 0.0f; //Freeze the action
            pauseWindowPrefab.gameObject.SetActive(true); //Spawn the pause menu window
            isGamePaused = true; //Set the boolean value to signal the game is paused
        }
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

    public void QuitGame()
    {
        //Bye bye!
        Application.Quit();
    }
}
