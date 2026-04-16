using UnityEngine;

public class GetDay : MonoBehaviour
{
	[SerializeField] HeroDataManager heroData;

	public void GetCurrentDay()
    {
		heroData = FindAnyObjectByType<HeroDataManager>();
		GetComponentInChildren<TextMesh>().text = "Day " + heroData.GetDay().ToString();
		Destroy(this.gameObject, 3f);
	}
}
