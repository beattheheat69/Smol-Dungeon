using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Basic player movement to put on all controllable objects

    //Inputs
    PlayerInput playerInput;
    InputAction moveInput;

    //Speed
    public float speed = 5f;

    //Facing direction for movement and actions
    Vector2 moveDir;
    Vector2 lastDir;

    void Start()
    {
        //Input references
        playerInput = GetComponent<PlayerInput>();
        moveInput = playerInput.actions["Move"];
	}

	void Update()
    {
        //Reads Move input value and affects it to object transform
        moveDir = moveInput.ReadValue<Vector2>();

        //Keeps memory of last facing direction when there are no inputs
        if (moveDir != Vector2.zero)
            lastDir = moveDir;

        //Moves object via direction, speed and time
        transform.Translate(moveDir * speed * Time.deltaTime);

        //Flip sprite when moving left
        if (lastDir.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (lastDir.x > 0)
            GetComponent<SpriteRenderer>().flipX = false;
    }
}
