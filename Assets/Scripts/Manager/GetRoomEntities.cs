using UnityEngine;

public class GetRoomEntities : MonoBehaviour
{
	public GameObject twin;
	int crossbowCount = 0;

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
							//Add preset positions if crossbow
							if (child2.name == "Crossbow(Clone)")
							{
								switch (crossbowCount)
								{
									case 0:
										child2.transform.localPosition = new Vector2(-5, 2);
										break;
									case 1:
										child2.transform.localPosition = new Vector2(5, 2);
										break;
									case 2:
										child2.transform.localPosition = new Vector2(0, -2);
										break;
									default:
										break;
								}
								crossbowCount++;
							}
							else
							{
								float rndX = Random.Range(-6f, 6f);
								float rndY = Random.Range(-3f, 3f);
								child2.transform.localPosition = new Vector2(rndX, rndY);
							}
						}
					}
				}
			}
		}
	}
}
