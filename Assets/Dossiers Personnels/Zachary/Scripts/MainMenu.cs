using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button continueButton;
    [SerializeField] bool isContinueAvailable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isContinueAvailable)
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
    }

    public void LoadGame()
    {
        //MAGIC WORDS TO LOAD A NEW GAME OR CONTINUE FROM A PREVIOUS ONE
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
