using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Global Game Manager to handle most stuff

    //Inputs
    //PlayerInput playerInput;
    //InputAction pauseInput;

    //Pause menu stuff
    bool isPaused;
    public GameObject pauseMenu;

    [SerializeField]
    public GameObject[] heroes = new GameObject[2];

    public GameObject tuto;

    GameObject thingsToSend;

    [SerializeField] HeroDataManager heroData;
    public GameObject DaySplash;

	//public TMP_Text evilXPText;

	private void Start()
	{
        Invoke("StartSecondDay", 0.01f);
		//Input references
		//playerInput = GetComponent<PlayerInput>();
		//pauseInput = playerInput.actions["Pause"];
        //Time.timeScale = 0f; //Starts frozen, will unfreeze when closing tutorial panel
        //evilXPText.text = "EvilXP = " + EvilXPCount.GetXP().ToString();
        heroData = FindAnyObjectByType<HeroDataManager>();
        thingsToSend = GameObject.Find("ThingsToSend").gameObject;
	}

	private void Update()
	{
		//Call pause function when input pressed
		//Issue: Try to deactivate other inputs to prevent player actions while paused
		//if (pauseInput.WasPressedThisFrame())
		//	PauseGame();
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
		//Remove ThingsToSend
		if (thingsToSend != null)
			Destroy(thingsToSend);

		Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void StartTime()
    {
        Time.timeScale = 1f;
    }

    public void ShowDay()
    {
		DaySplash.GetComponent<Animator>().StartPlayback();
		//heroData = FindAnyObjectByType<HeroDataManager>();
		//DaySplash.GetComponentInChildren<TextMesh>().text = "What " + heroData.GetDay().ToString();
  //      GameObject inst = Instantiate(DaySplash, transform.position, Quaternion.identity);
  //      Destroy(inst, 3f);
    }

    void StartSecondDay()
    {
		heroData = FindAnyObjectByType<HeroDataManager>();
		if (heroData.GetDay() == 2)
        {
			tuto.SetActive(false);
            StartTime();
		}
        else
        {
            tuto.SetActive(true);
			Time.timeScale = 0f; //Starts frozen, will unfreeze when closing tutorial panel
            
            if (DaySplash != null)
                DaySplash.GetComponent<Animator>().StopPlayback();
		}
    }
}
