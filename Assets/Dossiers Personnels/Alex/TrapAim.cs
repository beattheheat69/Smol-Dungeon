using UnityEngine;
using UnityEngine.InputSystem;

public class TrapAim : MonoBehaviour
{
    //Follows the mouse pointer or joystick to aim

    PlayerInput playerInput;
    InputAction aim;
	Vector2 cursorPos;

	private void Start()
	{
		playerInput = GetComponentInParent<PlayerInput>();
		aim = playerInput.actions["Aim"];
	}

	private void FixedUpdate()
	{
		//Rotate to aim object/aim towards cursor
		cursorPos = Camera.main.ScreenToWorldPoint(aim.ReadValue<Vector2>());
		Debug.Log(cursorPos);
		//Vector2 lookDir = cursorPos - transform.position;
		//float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
		//transform.rotation = Quaternion.Euler(0, 0, angle);
	}
}
