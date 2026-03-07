using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterAction : MonoBehaviour
{
	//Script specific for monster actions

	//Inputs
	PlayerInput playerInput;
	InputAction basicActionInput;

	private void Start()
	{
		//Input references
		playerInput = GetComponent<PlayerInput>();
		basicActionInput = playerInput.actions["BasicAction"];
	}

	private void Update()
	{
		//Input and function call for basic action
		if (basicActionInput.WasPressedThisFrame())
			Debug.Log(this.gameObject.name + " attacks!");
	}
}
