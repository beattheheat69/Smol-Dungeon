using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Button continueButton;
    [SerializeField] bool isContinueAvailable;

    [SerializeField] GameObject pauseWindowPrefab;


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

        //FOR DEBUG PURPOSES (Test Pause Menu)

        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PopUpWindow(pauseWindowPrefab);
        }
    }

    public void LoadNewGame()
    {
        //Load the scene with the Dungeon Shaping Screen
        SceneManager.LoadScene("Dungeon Shape Screen");
    }

    public void LoadResumedGame()
    {
        //We'll get there when we get there
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PopUpWindow(GameObject window)
    {
        //Put the window in the center of the screen
        window.transform.localPosition = Vector3.zero;

        //Make the window appear
        window.SetActive(true);
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
