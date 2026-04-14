using System.Collections;
using TMPro;
using UnityEngine;

public class CountingUp : MonoBehaviour
{
    int XPcount;
    int uiXPcount;
	public TMP_Text uiXPtext;
	public float countUpSpeed = 0.01f;

	private void Start()
	{
		XPcount = EvilXPCount.GetXP();
		uiXPcount = EvilXPCount.GetXP();
	}

	public IEnumerator CountUp()
	{
		XPcount = EvilXPCount.GetXP();
		while (uiXPcount < XPcount)
		{
			uiXPcount++;
			uiXPtext.text = "Evil XP: " + uiXPcount.ToString();
			yield return new WaitForSecondsRealtime(countUpSpeed);
		}
	}
}
