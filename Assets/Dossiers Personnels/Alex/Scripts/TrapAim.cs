using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TrapAim : MonoBehaviour
{
    //Follows the mouse pointer or joystick to aim

    PlayerInput playerInput;
    InputAction aim;
	InputAction basicActionInput;
	public Vector2 cursorPos;
	public GameObject bullet;
	public float bulletVelocity = 5;

	private void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		aim = playerInput.actions["Aim"];
		basicActionInput = playerInput.actions["BasicAction"];
	}

	private void Update()
	{
		if (basicActionInput.WasPressedThisFrame())
			Shoot();
	}

	private void FixedUpdate()
	{
		//Rotate to aim object/aim towards cursor
		cursorPos = Camera.main.ScreenToWorldPoint(aim.ReadValue<Vector2>());

		cursorPos.x -= transform.position.x;
		cursorPos.y -= transform.position.y;
		var angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg - 90f;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

	public void Shoot()
	{
		//Cooldown
		GameObject inst = Instantiate(bullet, gameObject.transform.position, transform.rotation);
		inst.GetComponent<Rigidbody2D>().linearVelocity = cursorPos.normalized * bulletVelocity;
		Destroy(inst, 2f);
	}
}
