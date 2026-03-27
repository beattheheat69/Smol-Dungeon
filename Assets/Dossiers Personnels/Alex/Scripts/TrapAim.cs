using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TrapAim : TrapAction
{
    //Follows the mouse pointer or joystick to aim

    PlayerInput playerInput;
    InputAction aim;
	InputAction basicActionInput;
	public Vector2 cursorPos;
	public GameObject bullet;
	public float bulletVelocity = 5;
	public float cooldown = 0.5f;
	float timeForNextAttack = 0f;
    [SerializeField] CrossbowStats_SO baseStats;

    private void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		aim = playerInput.actions["Aim"];
		basicActionInput = playerInput.actions["BasicAction"];
	}

	private void Update()
	{
		//Cooldown timer
		if (timeForNextAttack < baseStats.attackCooldown) //Maybe individual cooldowns for each attack?
			timeForNextAttack += Time.deltaTime;

		if (basicActionInput.WasPressedThisFrame() && baseStats.attackCooldown <= timeForNextAttack)
		{
			Shoot();
			timeForNextAttack = 0;
		}
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
		//Animation
		GetComponent<Animator>().SetTrigger("Shoot");

		//Cooldown
		GameObject inst = Instantiate(bullet, gameObject.transform.position, transform.rotation);
		inst.GetComponent<Rigidbody2D>().linearVelocity = cursorPos.normalized * baseStats.arrowSpeed;
		Destroy(inst, 2f);
	}
}
