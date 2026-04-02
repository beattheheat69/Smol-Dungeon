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
