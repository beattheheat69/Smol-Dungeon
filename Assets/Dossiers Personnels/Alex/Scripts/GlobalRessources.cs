using TMPro;
using UnityEngine;

public class GlobalRessources : MonoBehaviour
{
    [SerializeField] int evilPoints = 12;
    public TMP_Text evilPointsScore;

	private void Start()
	{
		evilPointsScore.text = "Evil Points: " + evilPoints;
	}

	public void SpendEvilPoints(int amount)
    {
        evilPoints -= amount;
        evilPointsScore.text = "Evil Points: " + evilPoints;
    }

    public void GainEvilPoints(int amount)
    {
        evilPoints += amount;
		evilPointsScore.text = "Evil Points: " + evilPoints;
	}

    public int EvilPointsAmount()
    {
        return evilPoints;
    }
}
