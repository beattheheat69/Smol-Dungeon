using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPossession : MonoBehaviour
{
	//Checks for nearby entities for possession and identify closest one
	
	//Inputs
	PlayerInput playerInput;
	InputAction possessInput;

	//Layer masks and tags for references
	public LayerMask entityLayer;
	public string monsterTag;
	public string triggerMonsterTag;
	public string trapTag;

	//For scan around and store objects in list
	public float scanRadius = 1;
	GameObject closestEntity;
	public List<GameObject> entitiesNearMe = new List<GameObject>();

	//Lifebar
	[SerializeField] Lifebar lifebar;

	void Start()
    {
		//Grabs playerinput and Possess input action
        playerInput = GetComponent<PlayerInput>();
        possessInput = playerInput.actions["Possess"];
    }

    void Update()
    {
		//Calls Possessing when pressing the Possess input while nearby entities
        if (possessInput.WasReleasedThisFrame() && entitiesNearMe.Count > 0) //Released input so it won't overlap with possessed input
			Possess();
    }

	private void FixedUpdate()
	{
		//Adds highlight around nearest entity to indicate which is going to be possessed if input
		if (entitiesNearMe.Count > 0)
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
		if (other.CompareTag(monsterTag) || other.CompareTag(triggerMonsterTag) || other.CompareTag(trapTag))
			entitiesNearMe.Add(other.gameObject);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		//Removes from entity counter and entity list
		if (other.CompareTag(monsterTag) || other.CompareTag(triggerMonsterTag) || other.CompareTag(trapTag))
		{
			other.GetComponent<SpriteRenderer>().color = Color.white;	//Removes highlight before removing from list
			entitiesNearMe.Remove(other.gameObject);
		}
	}

	void Possess()	//Grabs closestEntity to enable control scripts then disable this script
	{
		if (closestEntity.GetComponent<EntityPossessed>().enabled == true)
			closestEntity.GetComponent<EntityPossessed>().Possessing(this.gameObject);
	}
}
