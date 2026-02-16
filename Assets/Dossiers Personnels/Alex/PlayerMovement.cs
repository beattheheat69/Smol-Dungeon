using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveInput;
    Vector2 moveDir;
    public float speed = 5f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveInput = playerInput.actions["Move"];
    }

    void Update()
    {
        moveDir = moveInput.ReadValue<Vector2>();

        transform.Translate(moveDir * speed * Time.deltaTime);
    }
}
