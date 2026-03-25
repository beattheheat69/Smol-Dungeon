using UnityEngine;

public class GetRoomEntities : MonoBehaviour
{
	public GameObject twin;

	private void Awake()
	{
		//Finds the info object from previous scene to affect entities in this room
		if (GameObject.Find("ThingsToSend") != null)
			twin = GameObject.Find("ThingsToSend").transform.Find(transform.name).gameObject;

		//Puts the entities in their respective child/parent with a random local pos
		if (twin != null)
		{
			while (twin.transform.childCount > 0)
			{
				foreach (Transform child in twin.transform)
				{
					child.parent = gameObject.transform;
					child.transform.localPosition = Vector2.zero;

					foreach (Transform child2 in child.transform)
					{
						//Exception for spikes trap
						if (child2.name != "SquareSpikeSmall(Clone)")
						{
							float rndX = Random.Range(-7f, 7f);
							float rndY = Random.Range(-4f, 4f);
							child2.transform.localPosition = new Vector2(rndX, rndY);
						}
					}
					//Go to other script to reference groups in code instead of inspector
				}
			}
		}
	}
}
