using TMPro;
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

    [SerializeField]
    public GameObject[] heroes = new GameObject[2];

    public GameObject tuto;

    //public TMP_Text evilXPText;

    private void Start()
	{
        tuto.SetActive(true);
		//Input references
		playerInput = GetComponent<PlayerInput>();
		pauseInput = playerInput.actions["Pause"];
        Time.timeScale = 0f; //Starts frozen, will unfreeze when closing tutorial panel
        //evilXPText.text = "EvilXP = " + EvilXPCount.GetXP().ToString();
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
        if (scene.name != "PlacementEntite")
        {
            ActivateHeroForDay();
        }

    }

    public void ActivateHeroForDay()
    {

        // Activate the hero for this day
        int index = HeroDataManager.Instance.GetDay() - 1;

        if (index >= 0 && index < heroes.Length)
            heroes[index].SetActive(true);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartTime()
    {
        Time.timeScale = 1f;
    }
}
