using UnityEngine;
using UnityEngine.InputSystem;

public class BossActions : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction basicAttack;
    InputAction specialAttackLeft;
    InputAction specialAttackRight;
	InputAction moveInput;
	public Vector2 moveDir;
	public Vector2 lastDir;
	public GameObject shockwaveAnim;

	private void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		basicAttack = playerInput.actions["BasicAction"];
		specialAttackLeft = playerInput.actions["SpecialLeft"];
		specialAttackRight = playerInput.actions["SpecialRight"];
		moveInput = playerInput.actions["Move"];

	}

	private void Update()
	{
		moveDir = moveInput.ReadValue<Vector2>();

		if (moveDir != Vector2.zero)
			lastDir = moveDir;

		if (basicAttack.WasPressedThisFrame())
			Debug.Log("Basic Attack");
		if (specialAttackLeft.WasPressedThisFrame())
		{
			Debug.Log("Special Attack Left");
			SpecialAttackLeft(lastDir);
		}
		if (specialAttackRight.WasPressedThisFrame())
		{
			Debug.Log("Special Attack Right");
			SpecialAttackRight(lastDir);
		}
	}

	void SpecialAttackLeft(Vector2 dir)
	{
		//Keep track of last direction
		GameObject inst = Instantiate(shockwaveAnim);
		inst.transform.position = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
		Destroy(inst, 2f);
	}

	void SpecialAttackRight(Vector2 dir)
	{

	}
}
