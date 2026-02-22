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

	void Start()
	{
		//Grabs references to playerinput and scripts
		playerInput = GetComponent<PlayerInput>();
		possessInput = playerInput.actions["Possess"];
		playerMovement = GetComponent<PlayerMovement>();
		entityPossessed = GetComponent<EntityPossessed>();
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
		playerMovement.enabled = false;
		entityPossessed.enabled = false;
	}
}
