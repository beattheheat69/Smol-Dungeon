using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPossession : MonoBehaviour
{
	PlayerInput playerInput;
	InputAction possessInput;
	int entityNearby = 0;
	public LayerMask entityLayer;
	public float scanRadius = 1;

	void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        possessInput = playerInput.actions["Interact"];
    }

    void Update()
    {
        if (possessInput.WasPressedThisFrame())
		{
			//Trying to find which is closer to player (might have to use array and compare)
			Collider2D[] entityHit = Physics2D.OverlapCircleAll(transform.position, scanRadius, entityLayer);
			foreach (Collider2D hit in entityHit)
			{
				//Find logic to check distance between entity found and player
				Debug.Log("Entity found!");
			}
		}
    }

	private void FixedUpdate()
	{
		//if (entityNearby > 0)
		//	Debug.Log("Press Y to possess");
		//Find a way to add highlight around nearest entity to indicate which is going to be possessed if input
		//Same with UI input icon
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Entity"))
        {
			entityNearby++;
        }
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Entity"))
		{
			entityNearby--;
		}
	}
}
