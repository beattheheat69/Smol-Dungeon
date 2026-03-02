using UnityEngine;
using UnityEngine.InputSystem;

public class EntityAction : MonoBehaviour
{
	PlayerInput playerInput;
	InputAction basicActionInput;

	private void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		basicActionInput = playerInput.actions["BasicAction"];
	}

	private void Update()
	{
		if (basicActionInput.WasPressedThisFrame())
		{
			Debug.Log(this.gameObject.name + " attacks!");
		}
	}
}
