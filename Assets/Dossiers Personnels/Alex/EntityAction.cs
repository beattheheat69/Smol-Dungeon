using UnityEngine;
using UnityEngine.InputSystem;

public class EntityAction : MonoBehaviour
{
	PlayerInput playerInput;
	InputAction basicActionInput;
	public TrapAim trapAim;
	bool trap;

	private void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		basicActionInput = playerInput.actions["BasicAction"];
		if (this.CompareTag("Trap"))
		{
			trap = true;
			trapAim = GetComponentInChildren<TrapAim>();
		}
	}

	private void Update()
	{
		if (basicActionInput.WasPressedThisFrame())
		{
			Debug.Log(this.gameObject.name + " attacks!");
			if (trap)
			{
				//Call TrapAim to pass cursorPos
				trapAim.Shoot();
			}
		}
	}
}
