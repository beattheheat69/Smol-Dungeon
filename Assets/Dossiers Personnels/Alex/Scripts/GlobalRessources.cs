using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalRessources : MonoBehaviour
{
    [SerializeField] int evilPoints = 12;
    public TMP_Text evilPointsScore;
    public Button confirmButton;
    public TMP_Text confirmButtonText;

	private void Start()
	{
		evilPointsScore.text = "Evil Points: " + evilPoints;
	}

	public void SpendEvilPoints(int amount)
    {
        evilPoints -= amount;
        evilPointsScore.text = "Evil Points: " + evilPoints;
        if (evilPoints <= 0)
        {
            confirmButton.interactable = true; //Replace with confirm prompt, player can proceed without spending all EP
            confirmButtonText.text = "Confirm";
        }
    }

    public void GainEvilPoints(int amount)
    {
        evilPoints += amount;
		evilPointsScore.text = "Evil Points: " + evilPoints;
		if (evilPoints > 0)
		{
			confirmButton.interactable = false;
			confirmButtonText.text = "Spend all Evil Points to proceed";
		}
	}

    public int EvilPointsAmount()
    {
        return evilPoints;
    }
}
