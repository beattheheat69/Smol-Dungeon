using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Basic player movement to put on all controllable objects

    //Inputs
    PlayerInput playerInput;
    InputAction moveInput;

    [SerializeField]
    MonsterStats_SO baseStats;

    //Speed
    public float speed = 5f;

    //Facing direction for movement and actions
    Vector2 moveDir;
    public Vector2 lastDir;

    void Start()
    {
        //Input references
        playerInput = GetComponent<PlayerInput>();
        moveInput = playerInput.actions["Move"];

        if (baseStats != null)
        {
            speed = baseStats.chargeSpeed;
        }
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

        //Flip sprite when moving left (but not for boss)
        if (this.gameObject.name != "Boss")
        {
            if (lastDir.x < 0)
                GetComponent<SpriteRenderer>().flipX = true;
            else if (lastDir.x > 0)
                GetComponent<SpriteRenderer>().flipX = false;
        }


        if (Camera.main.GetComponent<CameraManagement>() != null && Camera.main.GetComponent<CameraManagement>().GetTransitionning())
        {
            transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        }
    }

    public Vector2 GetDirection()
    {
        return moveDir;
    }
}
