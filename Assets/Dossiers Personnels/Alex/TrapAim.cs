using UnityEngine;
using UnityEngine.InputSystem;

public class TrapAim : MonoBehaviour
{
    //Follows the mouse pointer or joystick to aim

    PlayerInput playerInput;
    InputAction aim;
	public Vector2 cursorPos;
	public GameObject bullet;
	public float bulletVelocity = 5;

	private void Start()
	{
		playerInput = GetComponentInParent<PlayerInput>();
		aim = playerInput.actions["Aim"];
	}

	private void FixedUpdate()
	{
		//Rotate to aim object/aim towards cursor
		cursorPos = Camera.main.ScreenToWorldPoint(aim.ReadValue<Vector2>());

		cursorPos.x -= transform.position.x;
		cursorPos.y -= transform.position.y;
		var angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg - 90f;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		//transform.LookAt(cursorPos, Vector2.up);
		//Vector2 lookDir = cursorPos - transform.position;
		//float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
		//transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	public void Shoot()
	{
		//Cooldown
		GameObject inst = Instantiate(bullet, gameObject.transform.position, transform.rotation);
		inst.GetComponent<Rigidbody2D>().linearVelocity = cursorPos.normalized * bulletVelocity;
		Destroy(inst, 2f);
	}
}
