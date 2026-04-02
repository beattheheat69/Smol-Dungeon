using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Global Game Manager to handle most stuff

    //Inputs
    PlayerInput playerInput;
    InputAction pauseInput;

    //Pause menu stuff
    bool isPaused;
    public GameObject pauseMenu;

    int currentDay = 1;
    public GameObject[] heroes; 

	private void Start()
	{
		//Input references
		playerInput = GetComponent<PlayerInput>();
		pauseInput = playerInput.actions["Pause"];
	}

	private void Update()
	{
		//Call pause function when input pressed
		//Issue: Try to deactivate other inputs to prevent player actions while paused
		if (pauseInput.WasPressedThisFrame())
			PauseGame();
	}

	public void PauseGame()
    {
		//Sets time scale to 0 when pause and displays pause panel

        isPaused = !isPaused;

        if (isPaused)
        {
            //Affiche pause menu et stop time
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
			//Hide pause menu and start time
			pauseMenu.SetActive(false);
			Time.timeScale = 1;
		}
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ActivateHeroForDay(currentDay);
    }

    public void ActivateHeroForDay(int day)
    {
        // Disable all heroes
       /* foreach (GameObject hero in heroes)
            hero.SetActive(false);*/

        // Activate the hero for this day
        int index = day - 1;

        if (index >= 0 && index < heroes.Length)
            heroes[index].SetActive(true);
    }

    public void NextDay()
    {
        currentDay++;
    }

    public int GetDay()
    {
        return currentDay;
    }
}
