using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveInput;
    Vector2 moveDir;
    public float speed = 5f;
    bool lastMoveLeft;

    void Start()
    {
        //Grabs references to playerinput and Move input action
        playerInput = GetComponent<PlayerInput>();
        moveInput = playerInput.actions["Move"];
	}

	void Update()
    {
        //Reads Move input value and affects it to object transform
        moveDir = moveInput.ReadValue<Vector2>();
        transform.Translate(moveDir * speed * Time.deltaTime);
        //Flip sprite when moving left (need to remember last move dir to stay flipped if needed
        if (moveDir.x < 0 || lastMoveLeft)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;
    }
}
