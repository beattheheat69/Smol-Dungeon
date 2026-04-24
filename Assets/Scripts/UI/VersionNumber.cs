using TMPro;
using UnityEngine;

public class VersionNumber : MonoBehaviour
{
    TMP_Text versionText;

	private void Start()
	{
		versionText = GetComponent<TMP_Text>();
		versionText.text = Application.version;
	}
}
