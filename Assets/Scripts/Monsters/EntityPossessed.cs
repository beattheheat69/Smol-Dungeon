using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class EntityPossessed : MonoBehaviour
{
	//Is called when player possesses or unpossesses an entity

	//Inputs, objects and such
	PlayerInput playerInput;
	InputAction possessInput;
	public GameObject smol;
	bool isPossessed;

	//Local scripts references
	PlayerMovement playerMovement;
	MonsterAction monsterAction;
    TrapAction trapAction;
    TrapAI trapAI;
    MonsterAI monsterAI;
	SpikeAI spikeAI;
	SpikeControl spikeControl;

	//FMOD events
	public EventReference possessingSFX;
	public EventReference depossessingSFX;
	public Parameters musicParameter;

	[SerializeField] int monsterHealth;
	public Lifebar monsterLifeBar;
	public int getMonsterHealth;

	public GameObject possessVFX;
	public CanvasGroup inputIconAttack;
	public TMP_Text inputTextPossess;


	void Start()
	{
		//Grabs references to playerinput and scripts
		playerInput = GetComponent<PlayerInput>();
		possessInput = playerInput.actions["Possess"];
		if (this.gameObject.CompareTag("Monster") || this.gameObject.CompareTag("TriggerMonster"))
		{
			playerMovement = GetComponent<PlayerMovement>();
			monsterAction = GetComponent<MonsterAction>();
			monsterAI = GetComponent<MonsterAI>();
			monsterLifeBar = GameObject.FindGameObjectWithTag("UI").transform.Find("SmolHealthBar").GetComponent<Lifebar>();
		}
		else if (this.gameObject.CompareTag("Trap"))
		{
            trapAction = GetComponent<TrapAction>();
            trapAI = GetComponent<TrapAI>();
			spikeAI = GetComponent<SpikeAI>();
			spikeControl = GetComponent<SpikeControl>();
        }
		musicParameter = FindAnyObjectByType<Parameters>();
		inputIconAttack = GameObject.Find("Input_LMB").GetComponent<CanvasGroup>();
		inputTextPossess = GameObject.Find("Input_Spacebar").GetComponentInChildren<TMP_Text>();
	}

	void Update()
	{
		if (isPossessed && (this.gameObject.CompareTag("Monster") || this.gameObject.CompareTag("TriggerMonster")))
			monsterLifeBar.gameObject.SetActive(true);
		if (monsterLifeBar != null)
			monsterLifeBar.SetHealth(this.gameObject.GetComponent<Character>().GetCurrentHealth());

		if (this.GetComponent<Character>() != null && !this.GetComponent<Character>().IsAlive())
		{
            //DePossessing();
			this.enabled = false;
        }
			
		//Calls DePossessing when pressing the Possess input
		if (possessInput.WasPressedThisFrame() && isPossessed)
			DePossessing();

		if (Camera.main.GetComponent<CameraManagement>().GetTransitionning() || HeroParty.Instance.GetcutScene())
			DePossessing();
	}

	public void Possessing(GameObject playerSmol)
	{
		//Activates Smol game object at entity position and disables scripts from this entity
		isPossessed = true;
		smol = playerSmol;
		GameObject instVFX = Instantiate(possessVFX, smol.transform.position, Quaternion.identity);
		Destroy(instVFX, 1.0f);
		//smol.GetComponent<PlayerInput>().enabled = false;

		inputIconAttack.alpha = 1.0f;
		inputTextPossess.text = "Release";

		//Sets active all child game objects
		for (int i = 0; i < transform.childCount; i++)
			transform.GetChild(i).gameObject.SetActive(true);

		//Checks if entity is monster or trap for related scripts
		if ((this.gameObject.CompareTag("Monster") || this.gameObject.CompareTag("TriggerMonster")) && playerMovement != null && monsterAction != null && monsterAI != null)
		{
			playerMovement.enabled = true;
			monsterAction.enabled = true;
			monsterAI.enabled = false;
			monsterLifeBar = GameObject.FindGameObjectWithTag("UI").transform.Find("SmolHealthBar").GetComponent<Lifebar>();
			monsterLifeBar.gameObject.SetActive(true);
			if (this.gameObject.CompareTag("Monster"))
				monsterLifeBar.SetMaxHealth(GetComponent<SlimeAI>().baseStats.health);
			else if (this.gameObject.CompareTag("TriggerMonster"))
				monsterLifeBar.SetMaxHealth(GetComponent<ArmorAI>().baseStats.health);
		}
		else if (this.gameObject.CompareTag("Trap") && ((trapAction != null && trapAI != null) || (spikeControl != null && spikeAI != null)))
		{
			if (trapAction != null && trapAI != null)
			{
				trapAction.enabled = true;
				trapAI.enabled = false;
			}
			else if (spikeControl != null && spikeAI != null)
			{
				spikeControl.enabled = true;
				spikeAI.enabled = false;
			}

			if (this.gameObject.TryGetComponent<SpikeAI>(out SpikeAI script))
			{
				script.SetisPossesed(true);
			}
        }

		smol.SetActive(false);

		//Deactivate AI script

		//Call Possessing FMOD SFX
		RuntimeManager.PlayOneShot(possessingSFX);
		musicParameter.SetMax();

		playerInput.enabled = true;
		
	}

	public void DePossessing()
	{
		//Activates Smol game object at entity position and disables scripts from this entity
		isPossessed = false;


		if (smol != null)
		{
			GameObject instVFX = Instantiate(possessVFX, transform.position, Quaternion.identity);
			Destroy(instVFX, 1.0f);
			smol.SetActive(true);
			smol.transform.position = transform.position;
			RuntimeManager.PlayOneShot(depossessingSFX);
			musicParameter.SetNormal();
			inputIconAttack.alpha = 0.25f;
			inputTextPossess.text = "Possess";
			//smol.GetComponent<PlayerInput>().enabled = true;
			smol.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard&Mouse", playerInput.GetDevice<Keyboard>());
		}


		//Deactivate all child game objects
		for (int i = 0; i < transform.childCount; i++)
		{
            if (transform.GetChild(i).tag != "Trap")
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
			else
			{
				transform.GetChild(0).gameObject.SetActive(false);
			}
		}
		

        //Checks if entity is monster or trap for related scripts
        if ((this.gameObject.CompareTag("Monster") || this.gameObject.CompareTag("TriggerMonster")) && playerMovement != null && monsterAction != null && monsterAI != null)
		{
			playerMovement.enabled = false;
			monsterAction.enabled = false;
			monsterAI.enabled = true;
			if (monsterLifeBar != null)
			{
				monsterLifeBar.gameObject.SetActive(false);
				monsterLifeBar = null;
			}
		}
		else if (this.gameObject.CompareTag("Trap") && ((trapAction != null && trapAI != null) || (spikeControl != null && spikeAI != null)))
		{
			if (trapAction != null && trapAI != null)
			{
				trapAction.enabled = false;
				trapAI.enabled = true;
			}
			else if (spikeControl != null && spikeAI != null)
			{
				spikeControl.enabled = false;
				spikeAI.enabled = true;
			}

            if (this.gameObject.TryGetComponent<SpikeAI>(out SpikeAI script) && script.GetisPossesed())
            {
                script.SetisPossesed(false);
            }

        }

		smol = null;

		playerInput.enabled = false;
		
	}

	private void OnDisable()
	{
		DePossessing();
	}
}
