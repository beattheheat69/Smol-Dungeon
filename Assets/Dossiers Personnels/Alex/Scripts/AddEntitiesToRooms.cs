using UnityEngine;
using UnityEngine.SceneManagement;

public class AddEntitiesToRooms : MonoBehaviour
{
	Scene scene;

	private void Awake()
	{
		//SceneManager.sceneLoaded -= OnSceneLoaded;
		//SceneManager.sceneLoaded += OnSceneLoaded;

		scene = SceneManager.GetActiveScene();

		print(scene.path);

		if (scene.path == "Assets/Scenes/PremadeDungeonTest")
			AffectEntitiesToRooms();
	}

	//void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	//{
	//	if (scene.path != this.scene.path)
	//	{
	//		print("Bonne scene");
	//		AffectEntitiesToRooms();
	//	}
	//}

	void AffectEntitiesToRooms()
	{
		//Go through children and find parent object of same name (same room)
		//Put monstergroup and trapgroup in respective dungeonroom
		//Activate entities inside group (deactive monstergroup and trapgroup)

		foreach (Transform child in transform)
		{
			GameObject twin = GameObject.Find(child.name);
			foreach (Transform child2 in child.transform)
			{
				child2.transform.parent = twin.transform;
			}
		}
	}
}
