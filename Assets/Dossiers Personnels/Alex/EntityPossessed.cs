using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntityPossessed : MonoBehaviour
{
	//Re-activate Smol gameobject with same position as entity when de-possessing

	PlayerInput playerInput;
	InputAction possessInput;
	public GameObject smol;
	PlayerMovement playerMovement;
	EntityPossessed entityPossessed;
	EntityAction entityAction;

	void Start()
	{
		//Grabs references to playerinput and scripts
		playerInput = GetComponent<PlayerInput>();
		possessInput = playerInput.actions["Possess"];
		entityPossessed = GetComponent<EntityPossessed>();
		entityAction = GetComponent<EntityAction>();
		if (this.gameObject.CompareTag("Monster"))
			playerMovement = GetComponent<PlayerMovement>();
	}

	void Update()
	{
		//Calls DePossessing when pressing the Possess input
		if (possessInput.WasPressedThisFrame())
			DePossessing();
	}

	void DePossessing()
	{
		//Activates Smol game object at entity position and disables scripts from this entity
		smol.SetActive(true);
		smol.transform.position = transform.position;
		entityPossessed.enabled = false;
		entityAction.enabled = false;
		if (this.gameObject.CompareTag("Monster"))
			playerMovement.enabled = false;
	}
}
