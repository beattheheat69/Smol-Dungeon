using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button continueButton;
    [SerializeField] string newGameSceneString = "ConstructionMenu";
    [SerializeField] bool isContinueAvailable;


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
        SceneManager.LoadScene(newGameSceneString);
    }

    public void LoadResumedGame()
    {
        //We'll get there when we get there
    }

    public void PopUpWindow(GameObject window)
    {
        //Put the window in the center of the screen
        window.transform.localPosition = Vector3.zero;

        //Make the window appear
        window.SetActive(true);

    }

    public void QuitGame()
    {
        //Bye bye!
        Application.Quit();
    }
}
