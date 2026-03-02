using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPossession : MonoBehaviour
{
	PlayerInput playerInput;
	InputAction possessInput;
	int entityNearby = 0;
	public LayerMask entityLayer;
	public string monsterTag;
	public string trapTag;
	public float scanRadius = 1;
	GameObject closestEntity;
	List<GameObject> entitiesNearMe = new List<GameObject>();

	void Start()
    {
		//Grabs playerinput and Possess input action
        playerInput = GetComponent<PlayerInput>();
        possessInput = playerInput.actions["Possess"];
    }

    void Update()
    {
		//Calls Possessing when pressing the Possess input while nearby entities
        if (possessInput.WasPressedThisFrame() && entityNearby > 0)
			Possessing();
    }

	private void FixedUpdate()
	{
		//Adds highlight around nearest entity to indicate which is going to be possessed if input
		if (entityNearby > 0)
		{
			float nearestEntity = scanRadius + 1;	//To check only within range, +1 to avoid errors
			foreach (GameObject entity in entitiesNearMe)
			{
				entity.GetComponent<SpriteRenderer>().color = Color.white;
				if (Vector2.Distance(transform.position, entity.transform.position) < nearestEntity)
				{
					nearestEntity = Vector2.Distance(transform.position, entity.transform.position);
					closestEntity = entity;
				}
			}
			closestEntity.GetComponent<SpriteRenderer>().color = Color.green;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//Adds to entity counter and to entity list
		if (other.CompareTag(monsterTag) || other.CompareTag(trapTag))
        {
			entityNearby++;
			entitiesNearMe.Add(other.gameObject);
        }
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		//Removes from entity counter and entity list
		if (other.CompareTag(monsterTag) || other.CompareTag(trapTag))
		{
			entityNearby--;
			other.GetComponent<SpriteRenderer>().color = Color.white;	//Removes highlight before removing from list
			entitiesNearMe.Remove(other.gameObject);
		}
	}

	void Possessing()	//Grabs closestEntity to enable control scripts then disable this script
	{
		closestEntity.GetComponent<EntityPossessed>().enabled = true;
		closestEntity.GetComponent<EntityPossessed>().smol = this.gameObject;
		closestEntity.GetComponent<EntityAction>().enabled = true;
		if (closestEntity.CompareTag(monsterTag))
			closestEntity.GetComponent<PlayerMovement>().enabled = true;
		//***Deactivate AI script here***
		gameObject.SetActive(false);
	}
}
