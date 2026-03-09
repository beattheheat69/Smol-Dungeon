using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

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
	TrapAim trapAim;
	CrossbowAI crossbowAI;
	MonsterAI monsterAI;

	//FMOD events
	public EventReference possessingSFX;
	public EventReference depossessingSFX;


	void Start()
	{
		//Grabs references to playerinput and scripts
		playerInput = GetComponent<PlayerInput>();
		possessInput = playerInput.actions["Possess"];
		if (this.gameObject.CompareTag("Monster"))
		{
			playerMovement = GetComponent<PlayerMovement>();
			monsterAction = GetComponent<MonsterAction>();
			monsterAI = GetComponent<MonsterAI>();
		}
		else if (this.gameObject.CompareTag("Trap"))
		{
			trapAim = GetComponent<TrapAim>();
			crossbowAI = GetComponent<CrossbowAI>();
		}
	}

	void Update()
	{
		//Calls DePossessing when pressing the Possess input
		if (possessInput.WasPressedThisFrame() && isPossessed)
			DePossessing();

		if (Camera.main.GetComponent<CameraManagement>().GetTransitionning())
			DePossessing();
	}

	public void Possessing(GameObject playerSmol)
	{
		//Activates Smol game object at entity position and disables scripts from this entity
		isPossessed = true;
		smol = playerSmol;

		//Sets active all child game objects
		for (int i = 0; i < transform.childCount; i++)
			transform.GetChild(i).gameObject.SetActive(true);

		//Checks if entity is monster or trap for related scripts
		if (this.gameObject.CompareTag("Monster"))
		{
			playerMovement.enabled = true;
			monsterAction.enabled = true;
			monsterAI.enabled = false;
		}
		else if (this.gameObject.CompareTag("Trap"))
		{
			trapAim.enabled = true;
			crossbowAI.enabled = false;
		}

		smol.SetActive(false);

		//Deactivate AI script

		//Call Possessing FMOD SFX
		RuntimeManager.PlayOneShot(possessingSFX);
	}

	public void DePossessing()
	{
		//Activates Smol game object at entity position and disables scripts from this entity
		isPossessed = false;

		if (smol != null)
		{
			smol.SetActive(true);
			smol.transform.position = transform.position;
			RuntimeManager.PlayOneShot(depossessingSFX);
		}

		//Deactivate all child game objects
		for (int i = 0; i < transform.childCount; i++)
			transform.GetChild(i).gameObject.SetActive(false);

		//Checks if entity is monster or trap for related scripts
		if (this.gameObject.CompareTag("Monster"))
		{
			playerMovement.enabled = false;
			monsterAction.enabled = false;
			monsterAI.enabled = true;
		}
		else if (this.gameObject.CompareTag("Trap"))
		{
			trapAim.enabled = false;
			crossbowAI.enabled = true;
		}

		smol = null;

		//Activate AI script

		//Call DePossessing FMOD SFX
	}

	private void OnDisable()
	{
		DePossessing();
	}
}
