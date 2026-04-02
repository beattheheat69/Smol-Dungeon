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
}
