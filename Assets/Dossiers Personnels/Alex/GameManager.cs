using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    bool isPaused;
    public GameObject pauseMenu;
	PlayerInput playerInput;
	InputAction pauseInput;

	private void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		pauseInput = playerInput.actions["Pause"];
	}

	private void Update()
	{
		//Call pause function when input pressed
		//Try to deactivate other inputs to prevent player actions while paused
		if (pauseInput.WasPressedThisFrame())
			PauseGame();
	}

	public void PauseGame()
    {
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
